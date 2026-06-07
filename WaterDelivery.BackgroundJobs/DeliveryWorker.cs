using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Backend.Infrastructure.Persistence.DbEntities.TransactionOutbox;
using WaterDelivery.Contracts.Enums;

namespace WaterDelivery.BackgroundJobs;

/// <summary>
/// Periodically advances every in-flight delivery one step along its status pipeline:
/// Assembly -> TransferredDeliveryService -> WaitToDelivery -> Delivering -> IssuedToCourier -> Delivered.
///
/// Deliveries are discovered through the transactional outbox: when a delivery is created (or updated)
/// an Outbox row is written whose payload is the delivery id. The worker reads pending rows, moves the
/// matching delivery forward, persists it, and marks the row processed. Terminal deliveries
/// (Delivered / Rejected / Cancelled) are simply marked processed.
/// </summary>
public class DeliveryWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DeliveryWorker> _logger;

    public DeliveryWorker(IServiceProvider serviceProvider, ILogger<DeliveryWorker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var rand = new Random();

        while (!cancellationToken.IsCancellationRequested)
        {
            // Simulate variable processing cadence between 3 and 15 seconds.
            await Task.Delay(rand.Next(3, 15) * 1000, cancellationToken);

            try
            {
                await ProcessOnce(cancellationToken);
            }
            catch (Exception ex)
            {
                // Never let a single failed cycle kill the worker.
                _logger.LogError(ex, "Delivery worker cycle failed");
            }
        }
    }

    private async Task ProcessOnce(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var outboxRepository = scope.ServiceProvider.GetRequiredService<IOutboxRepository>();
        var deliveryRepository = scope.ServiceProvider.GetRequiredService<IDeliveryRepository>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var outboxes = await outboxRepository.GetUnprocessedOutboxes(cancellationToken);
        if (outboxes.Count == 0)
        {
            return;
        }

        _logger.LogInformation("Started processing {Count} outbox message(s)", outboxes.Count);

        foreach (var outbox in outboxes)
        {
            var reachedTerminal = await ProcessOutbox(outbox, deliveryRepository, cancellationToken);

            // Keep the row pending while the delivery still has steps left; only close it
            // out once the delivery is terminal (Delivered / Rejected / Cancelled) or gone.
            if (reachedTerminal)
            {
                outbox.Status = OutboxStatus.Processed;
                outbox.CompletedAt = DateTime.UtcNow;
            }

            outbox.UpdatedAt = DateTime.UtcNow;
            await outboxRepository.MarkProcessedAsync(outbox, cancellationToken);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Finished processing {Count} outbox message(s)", outboxes.Count);
    }

    private async Task<bool> ProcessOutbox(Outbox outbox, IDeliveryRepository deliveryRepository, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(outbox.PayLoad, out var deliveryId))
        {
            _logger.LogWarning("Outbox {OutboxId} has an unparseable payload, skipping", outbox.Id);
            return true; // nothing we can ever do with this row
        }

        Delivery delivery;
        try
        {
            delivery = await deliveryRepository.GetDeliveryByIdAsync(deliveryId, cancellationToken);
        }
        catch (InvalidOperationException)
        {
            _logger.LogWarning("Delivery {DeliveryId} referenced by outbox {OutboxId} no longer exists", deliveryId, outbox.Id);
            return true; // delivery gone -> close the row
        }

        var next = GetNextStatus(delivery.Status);
        if (next is null)
        {
            _logger.LogInformation("Delivery {DeliveryId} is in terminal status {Status}", delivery.Id, delivery.Status);
            return true;
        }

        delivery.SetStatus(next.Value);
        await deliveryRepository.UpdateDeliveryAsync(delivery, cancellationToken);
        _logger.LogInformation("Delivery {DeliveryId} advanced to {Status}", delivery.Id, next.Value);

        // Terminal if this step landed on the final state.
        return GetNextStatus(next.Value) is null;
    }

    /// <summary>
    /// Returns the next status in the happy-path pipeline, or null when the delivery
    /// is already in a terminal/branching state that the worker shouldn't auto-advance.
    /// Mirrors the transitions allowed by Delivery.SetStatus.
    /// </summary>
    private static DeliveryStatus? GetNextStatus(DeliveryStatus current) => current switch
    {
        DeliveryStatus.Assembly => DeliveryStatus.TransferredDeliveryService,
        DeliveryStatus.TransferredDeliveryService => DeliveryStatus.WaitToDelivery,
        DeliveryStatus.WaitToDelivery => DeliveryStatus.Delivering,
        DeliveryStatus.Delivering => DeliveryStatus.IssuedToCourier,
        DeliveryStatus.IssuedToCourier => DeliveryStatus.Delivered,
        _ => null
    };
}

using Mediator;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Backend.Infrastructure.Persistence.DbEntities.TransactionOutbox;
using WaterDelivery.Contracts.Deliveries.Dtos;

namespace WaterDelivery.Backend.Features.Deliveries;

public class CreateDeliveryUseCase: IRequestHandler<CreateDeliveryDto, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDeliveryRepository _deliveryRepository;
    private readonly IOutboxRepository _outboxRepository;

    public CreateDeliveryUseCase(IUnitOfWork unitOfWork, IDeliveryRepository deliveryRepository, IOutboxRepository outboxRepository)
    {
        _unitOfWork = unitOfWork;
        _deliveryRepository = deliveryRepository;
        _outboxRepository = outboxRepository;
    }
    
    public async ValueTask<Guid> Handle(CreateDeliveryDto request, CancellationToken cancellationToken)
    {
        var delivery = new Delivery(request.DeliveryManId, request.Order.ToEntity(), request.Address.ToEntity(),
            request.Status);
        var result = await _deliveryRepository.CreateDeliveryAsync(delivery, cancellationToken);
        var outbox = new Outbox
        {
            Id = Guid.NewGuid(),
            Status = OutboxStatus.Pending,
            PayLoad = delivery.Id.ToString(),
            UpdatedAt = DateTime.UtcNow,
            CompletedAt = null
        };
        await _outboxRepository.AddOutboxAsync(outbox, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return delivery.Id;
    }
}
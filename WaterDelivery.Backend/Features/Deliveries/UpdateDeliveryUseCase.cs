using Mediator;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Backend.Infrastructure.Persistence.DbEntities.TransactionOutbox;
using WaterDelivery.Contracts.Deliveries.Dtos;

namespace WaterDelivery.Backend.Features.Deliveries;

public class UpdateDeliveryUseCase: IRequestHandler<UpdateDeliveryDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDeliveryRepository _deliveryRepository;
    private readonly IOutboxRepository _outboxRepository;

    public UpdateDeliveryUseCase(IUnitOfWork unitOfWork, IDeliveryRepository deliveryRepository, IOutboxRepository outboxRepository)
    {
        _unitOfWork = unitOfWork;
        _deliveryRepository = deliveryRepository;
        _outboxRepository = outboxRepository;
    }
    
    public async ValueTask<Unit> Handle(UpdateDeliveryDto request, CancellationToken cancellationToken)
    {
        var deliveryToUpdate = new Delivery(request.Id, request.DeliveryManId, request.Order.ToEntity(),
            request.Address.ToEntity(), request.Status);
        await _deliveryRepository.UpdateDeliveryAsync(deliveryToUpdate, cancellationToken);
        var outbox = new Outbox
        {
            Id = Guid.NewGuid(),
            Status = OutboxStatus.Pending,
            PayLoad = deliveryToUpdate.Id.ToString(),
            UpdatedAt = DateTime.UtcNow,
            CompletedAt = null
        };
        await _outboxRepository.AddOutboxAsync(outbox, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
        ;
    }
}
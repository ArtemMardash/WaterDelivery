using Mediator;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.Deliveries.Dtos;
using WaterDelivery.Backend.Features.Shared;

namespace WaterDelivery.Backend.Features.Deliveries;

public class UpdateDeliveryUseCase: IRequestHandler<UpdateDeliveryDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDeliveryRepository _deliveryRepository;

    public UpdateDeliveryUseCase(IUnitOfWork unitOfWork, IDeliveryRepository deliveryRepository)
    {
        _unitOfWork = unitOfWork;
        _deliveryRepository = deliveryRepository;
    }
    
    public async ValueTask<Unit> Handle(UpdateDeliveryDto request, CancellationToken cancellationToken)
    {
        var deliveryToUpdate = new Delivery(request.Id, request.DeliveryManId, request.Order.ToEntity(),
            request.Address.ToEntity(), request.Status);
        await _deliveryRepository.UpdateDeliveryAsync(deliveryToUpdate, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
        ;
    }
}
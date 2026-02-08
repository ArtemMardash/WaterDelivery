using Mediator;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.Deliveries.Dtos;
using WaterDelivery.Backend.Features.Shared;

namespace WaterDelivery.Backend.Features.Deliveries;

public class CreateDeliveryUseCase: IRequestHandler<CreateDeliveryDto, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDeliveryRepository _deliveryRepository;

    public CreateDeliveryUseCase(IUnitOfWork unitOfWork, IDeliveryRepository deliveryRepository)
    {
        _unitOfWork = unitOfWork;
        _deliveryRepository = deliveryRepository;
    }
    
    public async ValueTask<Guid> Handle(CreateDeliveryDto request, CancellationToken cancellationToken)
    {
        var delivery = new Delivery(request.DeliveryManId, request.Order.ToEntity(), request.Address.ToEntity(),
            request.Status);
        var result = await _deliveryRepository.CreateDeliveryAsync(delivery, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return delivery.Id;
    }
}
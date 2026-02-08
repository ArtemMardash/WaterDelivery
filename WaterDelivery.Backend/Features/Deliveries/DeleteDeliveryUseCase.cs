using Mediator;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.Deliveries.Dtos;
using WaterDelivery.Backend.Features.Shared;

namespace WaterDelivery.Backend.Features.Deliveries;

public class DeleteDeliveryUseCase: IRequestHandler<DeleteDeliveryDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDeliveryRepository _deliveryRepository;

    public DeleteDeliveryUseCase(IUnitOfWork unitOfWork, IDeliveryRepository deliveryRepository)
    {
        _unitOfWork = unitOfWork;
        _deliveryRepository = deliveryRepository;
    }
    
    public async ValueTask<Unit> Handle(DeleteDeliveryDto request, CancellationToken cancellationToken)
    {
        await _deliveryRepository.DeleteDeliveryAsync(request.Id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
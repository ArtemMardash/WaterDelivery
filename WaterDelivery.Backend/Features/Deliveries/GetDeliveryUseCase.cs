using Mediator;
using WaterDelivery.Backend.Features.Deliveries.Dtos;
using WaterDelivery.Backend.Features.Shared;

namespace WaterDelivery.Backend.Features.Deliveries;

public class GetDeliveryUseCase: IRequestHandler<GetDeliveryDto, GetDeliveryResultDto>
{
    private readonly IDeliveryRepository _deliveryRepository;

    public GetDeliveryUseCase(IDeliveryRepository deliveryRepository)
    {
        _deliveryRepository = deliveryRepository;
    }
    public async ValueTask<GetDeliveryResultDto> Handle(GetDeliveryDto request, CancellationToken cancellationToken)
    {
        var result = await _deliveryRepository.GetDeliveryByIdAsync(request.Id, cancellationToken);
        return new GetDeliveryResultDto
        {
            Id = result.Id,
            DeliveryManId = result.DeliveryManId,
            Order = result.Order.ToDto(),
            Address = result.Address.ToDto(),
            Status = result.Status
        };
    }
}
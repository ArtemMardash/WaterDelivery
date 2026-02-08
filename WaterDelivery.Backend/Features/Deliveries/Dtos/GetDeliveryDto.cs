using Mediator;

namespace WaterDelivery.Backend.Features.Deliveries.Dtos;

public class GetDeliveryDto: IRequest<GetDeliveryResultDto>
{
    public Guid Id { get; set; }
}
using Mediator;

namespace WaterDelivery.Contracts.Deliveries.Dtos;

public class GetDeliveryDto: IRequest<GetDeliveryResultDto>
{
    public Guid Id { get; set; }
}
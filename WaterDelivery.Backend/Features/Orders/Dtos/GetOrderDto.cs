using Mediator;

namespace WaterDelivery.Backend.Features.Orders.Dtos;

public class GetOrderDto: IRequest<GetOrderResultDto>
{
    public Guid Id { get; set; }
}
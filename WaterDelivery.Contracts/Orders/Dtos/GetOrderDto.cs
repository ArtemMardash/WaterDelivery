using Mediator;

namespace WaterDelivery.Contracts.Orders.Dtos;

public class GetOrderDto: IRequest<GetOrderResultDto>
{
    public Guid Id { get; set; }
}
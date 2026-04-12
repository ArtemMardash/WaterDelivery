using Mediator;

namespace WaterDelivery.Contracts.Orders.Dtos;

public class CreateOrderDto: IRequest<Guid>
{
    public Guid CustomerId { get; set; }

    public List<OrderItemDto> Items { get; set; } 
}
using Mediator;

namespace WaterDelivery.Backend.Features.Orders.Dtos;

public class CreateOrderDto: IRequest<Guid>
{
    public Guid CustomerId { get; set; }

    public List<OrderItemDto> Items { get; set; } 
}
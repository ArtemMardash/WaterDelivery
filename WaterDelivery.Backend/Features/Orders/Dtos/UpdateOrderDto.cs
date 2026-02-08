using Mediator;

namespace WaterDelivery.Backend.Features.Orders.Dtos;

public class UpdateOrderDto: IRequest
{
    public Guid Id { get; set; }
    
    public Guid CustomerId { get; set; }

    public List<OrderItemDto> Items { get; set; } 
}
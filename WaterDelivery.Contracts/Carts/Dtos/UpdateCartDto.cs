using Mediator;

namespace WaterDelivery.Contracts.Carts.Dtos;

public class UpdateCartDto: IRequest
{
    public Guid CustomerId { get; set; }
    
    public Dictionary<Guid, int> Items { get; set; }
}
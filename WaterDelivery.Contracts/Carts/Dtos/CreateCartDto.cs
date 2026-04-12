using Mediator;

namespace WaterDelivery.Contracts.Carts.Dtos;

public class CreateCartDto: IRequest<Guid>
{
    public Guid CustomerId { get; set; }
    
    public Dictionary<Guid, int> Items { get; set; }
}
using Mediator;

namespace WaterDelivery.Contracts.Carts.Dtos;

public class GetCartResultDto: IRequest
{
    public Guid CustomerId { get; set; }
    
    public Dictionary<Guid, int> Items { get; set; }
    
    public decimal TotalPrice { get; set; }
}
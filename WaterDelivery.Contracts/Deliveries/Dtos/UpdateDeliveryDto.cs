using Mediator;
using WaterDelivery.Contracts.Addresses.Dtos;
using WaterDelivery.Contracts.Enums;
using WaterDelivery.Contracts.Orders.Dtos;

namespace WaterDelivery.Contracts.Deliveries.Dtos;

public class UpdateDeliveryDto: IRequest
{
    public Guid Id { get; set; }
    
    public Guid DeliveryManId { get; set; }
    
    public OrderDto Order { get; set; }
    
    public AddressDto Address { get; set; }
    
    public DeliveryStatus Status { get; set; }

}
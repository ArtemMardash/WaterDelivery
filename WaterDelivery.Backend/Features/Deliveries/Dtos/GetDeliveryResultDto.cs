using Mediator;
using WaterDelivery.Backend.Core.Enums;
using WaterDelivery.Backend.Features.Addresses.Dtos;
using WaterDelivery.Backend.Features.Orders.Dtos;

namespace WaterDelivery.Backend.Features.Deliveries.Dtos;

public class GetDeliveryResultDto: IRequest
{
    public Guid Id { get; set; }
    
    public Guid DeliveryManId { get; set; }
    
    public OrderDto Order { get; set; }
    
    public AddressDto Address { get; set; }
    
    public DeliveryStatus Status { get; set; }
}
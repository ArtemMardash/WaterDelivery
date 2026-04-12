using WaterDelivery.Contracts.Orders.Dtos;
using WaterDelivery.UI.Components.Catalog;

namespace WaterDelivery.UI.Components.PlaceOrder;

public class PlaceOrderRequest
{
    public Guid CustomerId { get; set; }

    public string Street { get; set; } = string.Empty;
    public string HouseNumber { get; set; } = string.Empty;
    public string AptNumber { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;

    public List<OrderItemDto> Items { get; set; } = new();
}

public class PlaceOrderResult
{
    public Guid AddressId { get; set; }
    
    public Guid BillId { get; set; }
    
    public Guid OrderId { get; set; }
}
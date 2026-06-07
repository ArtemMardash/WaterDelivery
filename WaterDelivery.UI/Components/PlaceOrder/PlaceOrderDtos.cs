using WaterDelivery.Contracts.Addresses.Dtos;
using WaterDelivery.Contracts.Orders.Dtos;

namespace WaterDelivery.UI.Components.PlaceOrder;

public class PlaceOrderRequest
{
    public Guid CustomerId { get; set; }

    /// <summary>
    /// The address to deliver to. For a freshly typed address the Id may be Guid.Empty;
    /// the use-case will persist it to the customer's address book and use the resulting id.
    /// For a previously saved address this is fully populated (including Id).
    /// </summary>
    public AddressDto Address { get; set; } = new();

    /// <summary>
    /// True when the address was typed in this session (needs saving), false when the
    /// customer picked one of their existing saved addresses.
    /// </summary>
    public bool IsNewAddress { get; set; }

    public List<OrderItemDto> Items { get; set; } = new();
}

public class PlaceOrderResult
{
    public Guid AddressId { get; set; }

    public Guid BillId { get; set; }

    public Guid OrderId { get; set; }
}

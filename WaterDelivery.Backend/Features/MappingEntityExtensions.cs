using WaterDelivery.Backend.Core.Entities;

namespace WaterDelivery.Backend.Features;

public static class MappingEntityExtension
{
    public static Order ToOrder(this Cart cart, List<OrderItem> items)
    {
        return new Order(cart.CustomerId, items);
    }
}
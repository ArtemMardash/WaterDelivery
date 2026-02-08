namespace WaterDelivery.Backend.Core.Entities;

public class Order
{
    public Guid Id { get; set; }
    
    public Guid CustomerId { get; set; }

    public List<OrderItem> Items { get; set; } 

    public Order(Guid id, Guid customerId, List<OrderItem> orderItems)
    {
        Id = id;
        CustomerId = customerId;
        Items = orderItems ?? new List<OrderItem>();
    }
    
    public Order(Guid customerId, List<OrderItem> orderItems)
    {
        Id = Guid.NewGuid();
        CustomerId = customerId;
        Items = orderItems ?? new List<OrderItem>();
    }
    
}

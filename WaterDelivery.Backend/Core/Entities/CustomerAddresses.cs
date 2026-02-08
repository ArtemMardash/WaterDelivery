namespace WaterDelivery.Backend.Core.Entities;

public class CustomerAddresses
{
    public Guid Id { get; set; }
    
    public Guid CustomerId { get; set; }

    public List<Address> Addresses { get; set; } = new List<Address>();

    public CustomerAddresses(Guid id, Guid customerId, List<Address> addresses)
    {
        Id = id;
        CustomerId = customerId;
        Addresses = addresses;
    }
    
    public CustomerAddresses(Guid customerId, List<Address> addresses)
    {
        Id = Guid.NewGuid();
        CustomerId = customerId;
        Addresses = addresses;
    }
}
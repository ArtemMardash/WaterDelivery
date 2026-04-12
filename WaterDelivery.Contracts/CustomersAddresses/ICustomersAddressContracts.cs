using WaterDelivery.Contracts.Addresses.Dtos;

namespace WaterDelivery.Contracts.CustomersAddresses;

public interface ICreateCustomerAddresses
{
    public Guid CustomerId { get; set; }

    public List<AddressDto> Addresses { get; set; }
}

public interface IGetCustomerAddresses
{
    public Guid CustomerId { get; set; }
}
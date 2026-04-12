using Mediator;
using WaterDelivery.Contracts.Addresses.Dtos;

namespace WaterDelivery.Contracts.CustomersAddresses.Dtos;

public class UpdateCustomerAddressesDto: IRequest
{
    public Guid Id { get; set; }
    
    public Guid CustomerId { get; set; }

    public List<AddressDto> Addresses { get; set; } = new();
}
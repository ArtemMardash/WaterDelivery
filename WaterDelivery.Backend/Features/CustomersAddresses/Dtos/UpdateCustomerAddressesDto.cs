using Mediator;
using WaterDelivery.Backend.Features.Addresses.Dtos;

namespace WaterDelivery.Backend.Features.CustomersAddresses.Dtos;

public class UpdateCustomerAddressesDto: IRequest
{
    public Guid Id { get; set; }
    
    public Guid CustomerId { get; set; }

    public List<AddressDto> Addresses { get; set; } = new();
}
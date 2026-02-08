using Mediator;
using WaterDelivery.Backend.Features.Addresses.Dtos;

namespace WaterDelivery.Backend.Features.CustomersAddresses.Dtos;

public class CreateCustomerAddressesDto: IRequest<Guid>
{
    public Guid CustomerId { get; set; }

    public List<AddressDto> Addresses { get; set; } = new List<AddressDto>();
}
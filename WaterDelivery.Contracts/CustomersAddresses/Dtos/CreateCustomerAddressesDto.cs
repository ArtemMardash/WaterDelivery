using Mediator;
using WaterDelivery.Contracts.Addresses.Dtos;

namespace WaterDelivery.Contracts.CustomersAddresses.Dtos;

public class CreateCustomerAddressesDto: IRequest<Guid>, ICreateCustomerAddresses
{
    public Guid CustomerId { get; set; }

    public List<AddressDto> Addresses { get; set; } = new List<AddressDto>();
}
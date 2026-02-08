using WaterDelivery.Backend.Features.Addresses.Dtos;

namespace WaterDelivery.Backend.Features.CustomersAddresses.Dtos;

public class GetCustomerAddressesResultDto
{
    public Guid Id { get; set; }
    
    public Guid CustomerId { get; set; }

    public List<AddressDto> Addresses { get; set; } = new List<AddressDto>();
}
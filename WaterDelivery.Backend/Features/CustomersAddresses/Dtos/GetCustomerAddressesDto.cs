using Mediator;

namespace WaterDelivery.Backend.Features.CustomersAddresses.Dtos;

public class GetCustomerAddressesDto: IRequest<GetCustomerAddressesResultDto>
{
    public Guid Id { get; set; }
}
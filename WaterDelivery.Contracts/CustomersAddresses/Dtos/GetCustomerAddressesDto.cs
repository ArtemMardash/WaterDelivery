using Mediator;

namespace WaterDelivery.Contracts.CustomersAddresses.Dtos;

public class GetCustomerAddressesDto: IRequest<GetCustomerAddressesResultDto>, IGetCustomerAddresses
{
    public Guid CustomerId { get; set; }
}
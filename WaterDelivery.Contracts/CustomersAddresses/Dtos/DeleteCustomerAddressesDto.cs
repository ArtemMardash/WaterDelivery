using Mediator;

namespace WaterDelivery.Contracts.CustomersAddresses.Dtos;

public class DeleteCustomerAddressesDto: IRequest
{
    public Guid Id { get; set; }
}
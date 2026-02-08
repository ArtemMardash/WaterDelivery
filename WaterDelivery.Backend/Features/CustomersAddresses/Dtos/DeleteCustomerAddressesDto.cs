using Mediator;

namespace WaterDelivery.Backend.Features.CustomersAddresses.Dtos;

public class DeleteCustomerAddressesDto: IRequest
{
    public Guid Id { get; set; }
}
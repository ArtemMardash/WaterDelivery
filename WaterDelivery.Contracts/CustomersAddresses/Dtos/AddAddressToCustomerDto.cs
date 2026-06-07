using Mediator;
using WaterDelivery.Contracts.Addresses.Dtos;

namespace WaterDelivery.Contracts.CustomersAddresses.Dtos;

/// <summary>
/// Adds a single address to the customer's address book.
/// If the customer has no address book yet, one is created.
/// Returns the id of the address that now lives in the book
/// (either the newly added one, or the existing matching address).
/// </summary>
public class AddAddressToCustomerDto : IRequest<Guid>
{
    public Guid CustomerId { get; set; }

    public AddressDto Address { get; set; } = new();
}

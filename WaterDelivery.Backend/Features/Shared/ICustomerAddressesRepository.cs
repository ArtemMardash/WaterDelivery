using WaterDelivery.Backend.Core.Entities;

namespace WaterDelivery.Backend.Features.Shared;

public interface ICustomerAddressesRepository
{
    public Task<Guid> CreateCustomerAddressesAsync(CustomerAddresses customerAddresses, CancellationToken cancellationToken);

    public Task UpdateCustomerAddressesAsync(CustomerAddresses customerAddresses, CancellationToken cancellationToken);

    public Task<CustomerAddresses> GetCustomerAddressesByIdAsync(Guid id, CancellationToken cancellationToken);

    public Task<List<Address>> GetAllCustomerAddresses(Guid customerId, CancellationToken cancellationToken);

    public Task DeleteCustomerAddressesAsync(Guid id, CancellationToken cancellationToken);
}
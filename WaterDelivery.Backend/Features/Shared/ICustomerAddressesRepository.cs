using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Core.ValueObjects;
using WaterDelivery.Backend.Infrastructure.Persistence.DbEntities;

namespace WaterDelivery.Backend.Features.Shared;

public interface ICustomerAddressesRepository
{
    public Task<string> CreateCustomerAddressesAsync(CustomerAddresses customerAddresses, CancellationToken cancellationToken);

    public Task UpdateCustomerAddressesAsync(CustomerAddresses customerAddresses, CancellationToken cancellationToken);

    public Task<CustomerAddresses> GetCustomerAddressesByIdAsync(Guid id, CancellationToken cancellationToken);

    public Task<List<Address>> GetAllCustomerAddresses(Guid customerId, CancellationToken cancellationToken);

    public Task DeleteCustomerAddressesAsync(Guid id, CancellationToken cancellationToken);
}
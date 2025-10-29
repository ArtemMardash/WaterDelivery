using MongoDB.Driver;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Core.ValueObjects;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Backend.Infrastructure.Persistence.DbEntities;
using WaterDelivery.Backend.Infrastructure.Persistence.Mapping;

namespace WaterDelivery.Backend.Infrastructure.Persistence.Repositories;

public class CustomerAddressesRepository : ICustomerAddressesRepository
{
    private readonly IMongoCollection<CustomerAddressesDb> _customerAddresses;

    public CustomerAddressesRepository(WaterDeliveryContext context)
    {
        _customerAddresses = context.GetCollection<CustomerAddressesDb>("customerAddresses");
    }

    public async Task<string> CreateCustomerAddressesAsync(CustomerAddresses customerAddresses,
        CancellationToken cancellationToken)
    {
        var customerAddressesDb = customerAddresses.ToDb();
        await _customerAddresses.InsertOneAsync(customerAddressesDb, cancellationToken: cancellationToken);

        return customerAddressesDb.Id;
    }

    public async Task UpdateCustomerAddressesAsync(CustomerAddresses customerAddresses,
        CancellationToken cancellationToken)
    {
        await _customerAddresses.ReplaceOneAsync(c => c.Id == customerAddresses.Id.ToString(), customerAddresses.ToDb(),
            cancellationToken: cancellationToken);
    }

    public async Task<CustomerAddresses> GetCustomerAddressesByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var customerAddressDb =
            await _customerAddresses.Find(c => c.Id == id.ToString()).FirstOrDefaultAsync(cancellationToken);

        return customerAddressDb.ToDomain();
    }

    public async Task<List<Address>> GetAllCustomerAddresses(Guid customerId, CancellationToken cancellationToken)
    {
        var customerAddresses =
            await _customerAddresses.Find(c => c.CustomerId == customerId).FirstOrDefaultAsync(cancellationToken);

        return customerAddresses.Addresses.Select(a => a.ToDomain()).ToList();
    }

    public async Task DeleteCustomerAddressesAsync(Guid id, CancellationToken cancellationToken)
    {
        await _customerAddresses.DeleteOneAsync(c => c.Id == id.ToString(), cancellationToken);
    }
}
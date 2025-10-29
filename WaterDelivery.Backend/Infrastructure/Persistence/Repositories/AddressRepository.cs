using MongoDB.Driver;
using WaterDelivery.Backend.Core.ValueObjects;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Backend.Infrastructure.Persistence.DbEntities;
using WaterDelivery.Backend.Infrastructure.Persistence.Mapping;

namespace WaterDelivery.Backend.Infrastructure.Persistence.Repositories;

public class AddressRepository: IAddressRepository
{
    private readonly IMongoCollection<AddressDb> _address;
    
    public AddressRepository(WaterDeliveryContext context)
    {
        _address = context.GetCollection<AddressDb>("address");
    }
    
    public async Task<string> CreateAddressAsync(Address address, CancellationToken cancellationToken)
    {
        var addressDb = address.ToDb();
        await _address.InsertOneAsync(addressDb, cancellationToken: cancellationToken);

        return addressDb.Id;
    }

    public async Task UpdateAddressAsync(Address address, CancellationToken cancellationToken)
    {
        await _address.ReplaceOneAsync(a => a.Id == address.Id.ToString(), address.ToDb(),
            cancellationToken: cancellationToken);
    }

    public async Task<Address> GetAddressAsync(Guid id, CancellationToken cancellationToken)
    {
        var addressDb = await _address.Find(a => a.Id == id.ToString()).FirstOrDefaultAsync(cancellationToken);

        return addressDb.ToDomain();
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await _address.DeleteOneAsync(a => a.Id == id.ToString(), cancellationToken);
    }
}
using MongoDB.Driver;
using WaterDelivery.Backend.Core.Entities;
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
    
    public async Task<Guid> CreateAddressAsync(Address address, CancellationToken cancellationToken)
    {
        var addressDb = address.ToDb();
        await _address.InsertOneAsync(addressDb, cancellationToken: cancellationToken);

        return addressDb.Id;
    }

    public async Task UpdateAddressAsync(Address address, CancellationToken cancellationToken)
    {
        var db = address.ToDb();
        var update = Builders<AddressDb>.Update
            .Set(a => a.City, db.City)
            .Set(a => a.State, db.State)
            .Set(a => a.Street, db.Street)
            .Set(a => a.AptNumber, db.AptNumber)
            .Set(a => a.HouseNumber, db.HouseNumber);
        await _address.UpdateOneAsync(a=>a.Id == address.Id, update, cancellationToken: cancellationToken);
    }

    public async Task<Address> GetAddressAsync(Guid id, CancellationToken cancellationToken)
    {
        var addressDb = await _address.Find(a => a.Id == id && a.isDeleted == false).FirstOrDefaultAsync(cancellationToken);
        if (addressDb == null)
        {
            throw new InvalidOperationException("There is no Address with such Id");
        }

        return addressDb.ToDomain();
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var update = Builders<AddressDb>.Update.Set(a => a.isDeleted, true);

        await _address.UpdateOneAsync(a => a.Id == id && a.isDeleted == false, update,
            cancellationToken: cancellationToken);
    }
}
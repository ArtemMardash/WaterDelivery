using MongoDB.Bson.Serialization.Attributes;

namespace WaterDelivery.Backend.Infrastructure.Persistence.DbEntities;

public class CustomerAddressesDb
{
    [BsonId]
    public Guid Id { get; set; }
    
    public Guid CustomerId { get; set; }

    public List<AddressDb> Addresses { get; set; } = new List<AddressDb>();
}
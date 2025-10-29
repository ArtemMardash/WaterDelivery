using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WaterDelivery.Backend.Infrastructure.Persistence.DbEntities;

public class CustomerAddressesDb
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    
    public Guid CustomerId { get; set; }

    public List<AddressDb> Addresses { get; set; } = new List<AddressDb>();
}
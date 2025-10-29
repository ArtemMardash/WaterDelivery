using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WaterDelivery.Backend.Infrastructure.Persistence.DbEntities;

public class DeliveryDb
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    
    public Guid DeliveryManId { get; set; }
    
    public OrderDb Order { get; set; }
    
    public AddressDb Address { get; set; }
    
    public int Status { get; set; }
}
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WaterDelivery.Backend.Infrastructure.Persistence.DbEntities;

public class ProductUnitDb
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    
    public int Name { get; set; }
    
    public int QuantityPerUnit { get; set; }
}
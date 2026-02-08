using MongoDB.Bson.Serialization.Attributes;

namespace WaterDelivery.Backend.Infrastructure.Persistence.DbEntities;

public class ProductUnitDb
{
    [BsonId]
    public Guid Id { get; set; }
    
    public int Name { get; set; }
    
    public int QuantityPerUnit { get; set; }
}
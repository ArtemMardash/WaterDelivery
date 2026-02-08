using MongoDB.Bson.Serialization.Attributes;

namespace WaterDelivery.Backend.Infrastructure.Persistence.DbEntities;

public class ProductDb
{
    [BsonId]
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public List<ProductUnitDb> ProductOptions { get; set; }
    
    public ProductUnitDb DefaultUnit { get; set; }
    
    public decimal DefaultUnitPrice { get; set; }
}
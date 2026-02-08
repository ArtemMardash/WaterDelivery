using MongoDB.Bson.Serialization.Attributes;

namespace WaterDelivery.Backend.Infrastructure.Persistence.DbEntities;

public class BillDb
{
    [BsonId]
    public Guid Id { get; set; }
    
    public OrderDb Order { get; set; }
    
    public DateTime CreationDate { get; set; }
    
    public DateTime? PaymentDate { get; set; }
    
    public int Status { get; set; }
}
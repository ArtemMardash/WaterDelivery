using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WaterDelivery.Backend.Infrastructure.Persistence.DbEntities;

public class OrderDb
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    
    public Guid CustomerId { get; set; }

    public List<OrderItemDb> Items { get; set; } = new List<OrderItemDb>();
}
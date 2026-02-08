using MongoDB.Bson.Serialization.Attributes;

namespace WaterDelivery.Backend.Infrastructure.Persistence.DbEntities;

public class UserDb
{
    [BsonId]
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public int UserType { get; set; }
    
    public string Email { get; set; }
    
    public string PhoneNumber { get; set; }
}
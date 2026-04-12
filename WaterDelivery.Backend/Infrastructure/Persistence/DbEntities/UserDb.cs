using AspNetCore.Identity.Mongo.Model;

namespace WaterDelivery.Backend.Infrastructure.Persistence.DbEntities;

public class UserDb: MongoUser
{
    //[BsonId]
    public Guid Uid { get; set; }
    
    public string Name { get; set; }
    
    public int UserType { get; set; }
    
    //public string Email { get; set; }
    
    //public string PhoneNumber { get; set; }
    
    public string GoogleId { get; set; }
    
    public string GoogleRefreshToken { get; set; }
    
    public DateTime? GoogleRefreshTokenExpiry { get; set; }
    
    public string GoogleAccessToken { get; set; }
}
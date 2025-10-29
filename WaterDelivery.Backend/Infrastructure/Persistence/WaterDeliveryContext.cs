using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using WaterDelivery.Backend.Infrastructure.Persistence.DbEntities;

namespace WaterDelivery.Backend.Infrastructure.Persistence;

public class WaterDeliveryContext
{
    private readonly IMongoDatabase _database;

    public WaterDeliveryContext(string connectionString, string dbName)
    {
        var client = new MongoClient(connectionString);

        _database = client.GetDatabase(dbName);
    }

    public IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        return _database.GetCollection<T>(collectionName);
    }
}
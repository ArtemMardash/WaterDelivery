using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace WaterDelivery.Backend.Infrastructure.Persistence.DbEntities;

public class CartDb
{
    /// <summary>
    /// Customer Id
    /// </summary>
    [BsonId]
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Dictionary of products Ids and quantities
    /// </summary>
    [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
    public Dictionary<Guid, int> Items { get; set; } = new Dictionary<Guid, int>();
}
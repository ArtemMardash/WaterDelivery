using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace WaterDelivery.Tests;

public class BaseTest: IDisposable
{
    static BaseTest()
    {
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
    }

    public virtual void Dispose()
    {
        throw new NotImplementedException();
    }
}
using MongoDB.Driver;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Backend.Infrastructure.Persistence.DbEntities;
using WaterDelivery.Backend.Infrastructure.Persistence.Mapping;

namespace WaterDelivery.Backend.Infrastructure.Persistence.Repositories;

public class DeliveryRepsoitory: IDeliveryRepository
{
    private readonly IMongoCollection<DeliveryDb> _delivery;
    public DeliveryRepsoitory(WaterDeliveryContext context)
    {
        _delivery = context.GetCollection<DeliveryDb>("delivery");
    }
    
    public async Task<string> CreateDeliveryAsync(Delivery delivery, CancellationToken cancellationToken)
    {
        var deliveryDb = delivery.ToDb();
        await _delivery.InsertOneAsync(deliveryDb, cancellationToken: cancellationToken);

        return deliveryDb.Id;
    }

    public async Task UpdateDeliveryAsync(Delivery delivery, CancellationToken cancellationToken)
    {
        await _delivery.ReplaceOneAsync(delivery.Id.ToString(), delivery.ToDb(),
            cancellationToken: cancellationToken);
    }

    public async Task<Delivery> GetDeliveryByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var orderDb = await _delivery.Find(o => o.Id == id.ToString()).FirstOrDefaultAsync(cancellationToken);

        return orderDb.ToDomain();
    }

    public async Task DeleteDeliveryAsync(Guid id, CancellationToken cancellationToken)
    {
        await _delivery.DeleteOneAsync(d=>d.Id == id.ToString(), cancellationToken: cancellationToken);
    }
}
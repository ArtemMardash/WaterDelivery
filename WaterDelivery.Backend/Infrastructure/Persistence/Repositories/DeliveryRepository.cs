using MongoDB.Driver;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Backend.Infrastructure.Persistence.DbEntities;
using WaterDelivery.Backend.Infrastructure.Persistence.Mapping;

namespace WaterDelivery.Backend.Infrastructure.Persistence.Repositories;

public class DeliveryRepository: IDeliveryRepository
{
    private readonly IMongoCollection<DeliveryDb> _delivery;
    public DeliveryRepository(WaterDeliveryContext context)
    {
        _delivery = context.GetCollection<DeliveryDb>("delivery");
    }
    
    public async Task<Guid> CreateDeliveryAsync(Delivery delivery, CancellationToken cancellationToken)
    {
        var deliveryDb = delivery.ToDb();
        await _delivery.InsertOneAsync(deliveryDb, cancellationToken: cancellationToken);

        return deliveryDb.Id;
    }

    public async Task UpdateDeliveryAsync(Delivery delivery, CancellationToken cancellationToken)
    {
        var db = delivery.ToDb();
        var update = Builders<DeliveryDb>.Update
            .Set(d => d.Address, db.Address)
            .Set(d => d.DeliveryManId, db.DeliveryManId)
            .Set(d => d.Status, db.Status)
            .Set(d => d.Order, db.Order);
        await _delivery.UpdateOneAsync(d => d.Id == delivery.Id, update, cancellationToken: cancellationToken);
    }

    public async Task<Delivery> GetDeliveryByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var orderDb = await _delivery.Find(o => o.Id == id).FirstOrDefaultAsync(cancellationToken);

        return orderDb.ToDomain();
    }

    public async Task DeleteDeliveryAsync(Guid id, CancellationToken cancellationToken)
    {
        await _delivery.DeleteOneAsync(d=>d.Id == id, cancellationToken: cancellationToken);
    }
}
using MongoDB.Driver;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Backend.Infrastructure.Persistence.DbEntities;
using WaterDelivery.Backend.Infrastructure.Persistence.Mapping;

namespace WaterDelivery.Backend.Infrastructure.Persistence.Repositories;

public class OrderRepository: IOrderRepository
{
    private readonly IMongoCollection<OrderDb> _order;

    public OrderRepository(WaterDeliveryContext context)
    {
        _order = context.GetCollection<OrderDb>("order");
    }
    
    public async Task<Guid> CreateOrderAsync(Order order, CancellationToken cancellationToken)
    {
        var orderdb = order.ToDb();
        await _order.InsertOneAsync(orderdb, cancellationToken);
        return orderdb.Id;
    }

    public async Task UpdateOrderAsync(Order order, CancellationToken cancellationToken)
    {
        var db = order.ToDb();
        var update = Builders<OrderDb>.Update
            .Set(o => o.CustomerId, db.CustomerId)
            .Set(o => o.Items, db.Items);
        await _order.UpdateOneAsync(o=>o.Id == db.Id, update, cancellationToken: cancellationToken);
    }

    public async Task<Order> GetOrderByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var filter = Builders<OrderDb>.Filter.Eq(o => o.Id, id);
        var orderDb = await _order.Find(filter).FirstOrDefaultAsync(cancellationToken);

        return orderDb != null ? orderDb.ToDomain() : throw new InvalidOperationException($"no such order with id {id}");
    }

    public async Task<List<Order>> GetAllCustomerOrdersAsync(Guid customerId, CancellationToken cancellationToken)
    {
        var orders = await _order.Find(o=>o.CustomerId == customerId).ToListAsync(cancellationToken);

        return orders.Select(o => o.ToDomain()).ToList();
    }

    public async Task DeleteOrderAsync(Guid id, CancellationToken cancellationToken)
    {
        await _order.DeleteOneAsync(o => o.Id == id, cancellationToken: cancellationToken);
    }
}
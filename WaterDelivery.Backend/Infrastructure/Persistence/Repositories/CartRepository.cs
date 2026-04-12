using MongoDB.Driver;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Backend.Infrastructure.Persistence.DbEntities;
using WaterDelivery.Backend.Infrastructure.Persistence.Mapping;

namespace WaterDelivery.Backend.Infrastructure.Persistence.Repositories;

public class CartRepository: ICartRepository
{
    private readonly IMongoCollection<CartDb> _cart;
    
    public CartRepository(WaterDeliveryContext context)
    {
        _cart = context.GetCollection<CartDb>("cart");
    }
    //ToDo check if the item still exists in db
    public async Task<Guid> CreateCartAsync(Cart cart, CancellationToken cancellationToken)
    {
        await _cart.InsertOneAsync(cart.ToDb(), cancellationToken: cancellationToken);

        return cart.CustomerId;
    }

    public Task UpdateCartAsync(Cart cart, CancellationToken cancellationToken)
    {
        return _cart.ReplaceOneAsync(c => c.CustomerId == cart.CustomerId, cart.ToDb(),
            cancellationToken: cancellationToken);
    }

    public async Task<Cart> GetCartAsync(Guid customerId, CancellationToken cancellationToken)
    {
        var cartDb = await _cart.Find(c => c.CustomerId == customerId).FirstOrDefaultAsync(cancellationToken);

        return cartDb.ToDomain();
    }
    
}
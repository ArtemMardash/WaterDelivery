using WaterDelivery.Backend.Core.Entities;

namespace WaterDelivery.Backend.Features.Shared;

public interface ICartRepository
{
    public Task<Guid> CreateCartAsync(Cart cart, CancellationToken cancellationToken);

    public Task UpdateCartAsync(Cart cart, CancellationToken cancellationToken);

    public Task<Cart> GetCartAsync(Guid customerId, CancellationToken cancellationToken);

}
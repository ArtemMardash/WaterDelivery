using WaterDelivery.Backend.Core.Entities;

namespace WaterDelivery.Backend.Features.Shared;

public interface IOrderRepository
{
    public Task<Guid> CreateOrderAsync(Order order, CancellationToken cancellationToken);

    public Task UpdateOrderAsync(Order order, CancellationToken cancellationToken);

    public Task<Order> GetOrderByIdAsync(Guid id, CancellationToken cancellationToken);

    public Task<List<Order>> GetAllCustomerOrdersAsync(Guid customerId, CancellationToken cancellationToken);

    public Task DeleteOrderAsync(Guid id, CancellationToken cancellationToken);
}
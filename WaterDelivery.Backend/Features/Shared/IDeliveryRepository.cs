using WaterDelivery.Backend.Core.Entities;

namespace WaterDelivery.Backend.Features.Shared;

public interface IDeliveryRepository
{
    public Task<string> CreateDeliveryAsync(Delivery delivery, CancellationToken cancellationToken);

    public Task UpdateDeliveryAsync(Delivery delivery, CancellationToken cancellationToken);

    public Task<Delivery> GetDeliveryByIdAsync(Guid id, CancellationToken cancellationToken);

    public Task DeleteDeliveryAsync(Guid id, CancellationToken cancellationToken);
}
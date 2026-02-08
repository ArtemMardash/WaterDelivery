using WaterDelivery.Backend.Core.Entities;

namespace WaterDelivery.Backend.Features.Shared;

public interface IAddressRepository
{
    public Task<Guid> CreateAddressAsync(Address address, CancellationToken cancellationToken);

    public Task UpdateAddressAsync(Address address, CancellationToken cancellationToken);

    public Task<Address> GetAddressAsync(Guid id, CancellationToken cancellationToken);

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
using WaterDelivery.Backend.Core.Entities;

namespace WaterDelivery.Backend.Features.Shared;

public interface IBillRepository
{
    public Task<Guid> CreateBillAsync(Bill bill, CancellationToken cancellationToken);

    public Task UpdateBillAsync(Bill bill, CancellationToken cancellationToken);

    public Task<Bill> GetBillByIdAsync(Guid id, CancellationToken cancellationToken);

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
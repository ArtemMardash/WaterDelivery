using WaterDelivery.Backend.Core.Entities;

namespace WaterDelivery.Backend.Features.Shared;

public interface IBillRepsoitory
{
    public Task<string> CreateBillAsync(Bill bill, CancellationToken cancellationToken);

    public Task UpdateBillAsync(Bill bill, CancellationToken cancellationToken);

    public Task<Bill> GetBillById(Guid id, CancellationToken cancellationToken);

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
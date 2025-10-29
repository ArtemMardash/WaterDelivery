using MongoDB.Driver;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Backend.Infrastructure.Persistence.DbEntities;
using WaterDelivery.Backend.Infrastructure.Persistence.Mapping;

namespace WaterDelivery.Backend.Infrastructure.Persistence.Repositories;

public class BillRepository: IBillRepsoitory
{
    private readonly IMongoCollection<BillDb> _bill;
    
    public BillRepository(WaterDeliveryContext context)
    {
        _bill = context.GetCollection<BillDb>("bill");
    }
    
    public async Task<string> CreateBillAsync(Bill bill, CancellationToken cancellationToken)
    {
        var billDb = bill.ToDb();
        await _bill.InsertOneAsync(billDb, cancellationToken);

        return billDb.Id;
    }

    public async Task UpdateBillAsync(Bill bill, CancellationToken cancellationToken)
    {
        await _bill.ReplaceOneAsync(b => b.Id == bill.Id.ToString(), bill.ToDb(), cancellationToken: cancellationToken);
    }

    public async Task<Bill> GetBillById(Guid id, CancellationToken cancellationToken)
    {
        var billDb = await _bill.Find(b => b.Id == id.ToString()).FirstOrDefaultAsync(cancellationToken);

        return billDb.ToDomain();
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await _bill.DeleteOneAsync(b => b.Id == id.ToString(), cancellationToken);
    }
}
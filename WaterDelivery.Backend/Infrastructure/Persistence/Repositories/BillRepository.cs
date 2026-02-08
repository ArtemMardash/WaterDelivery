using MongoDB.Driver;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Backend.Infrastructure.Persistence.DbEntities;
using WaterDelivery.Backend.Infrastructure.Persistence.Mapping;

namespace WaterDelivery.Backend.Infrastructure.Persistence.Repositories;

public class BillRepository: IBillRepository
{
    private readonly IMongoCollection<BillDb> _bill;
    
    public BillRepository(WaterDeliveryContext context)
    {
        _bill = context.GetCollection<BillDb>("bill");
    }
    
    public async Task<Guid> CreateBillAsync(Bill bill, CancellationToken cancellationToken)
    {
        var billDb = bill.ToDb();
        await _bill.InsertOneAsync(billDb, cancellationToken);

        return billDb.Id;
    }

    public async Task UpdateBillAsync(Bill bill, CancellationToken cancellationToken)
    {
        var billDb = bill.ToDb();
        var update = Builders<BillDb>.Update
            .Set(b => b.PaymentDate, billDb.PaymentDate)
            .Set(b => b.Order, billDb.Order)
            .Set(b => b.Status, billDb.Status)
            .Set(b => b.CreationDate, billDb.CreationDate);
        await _bill.UpdateOneAsync(b => b.Id == bill.Id, update, cancellationToken: cancellationToken);
    }

    public async Task<Bill> GetBillByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var billDb = await _bill.Find(b => b.Id == id).FirstOrDefaultAsync(cancellationToken);

        return billDb.ToDomain();
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await _bill.DeleteOneAsync(b => b.Id == id, cancellationToken);
    }
}
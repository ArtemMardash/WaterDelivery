using MongoDB.Driver;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Backend.Infrastructure.Persistence.DbEntities.TransactionOutbox;

namespace WaterDelivery.Backend.Infrastructure.Persistence.Repositories;

public class OutboxRepository: IOutboxRepository
{
    private readonly IMongoCollection<Outbox> _outbox;

    public OutboxRepository(WaterDeliveryContext context)
    {
        _outbox = context.GetCollection<Outbox>("Outboxes");
    }

    public async Task<List<Outbox>> GetUnprocessedOutboxes(CancellationToken cancellationToken)
    {
        var filter = Builders<Outbox>.Filter.Eq(o => o.Status, OutboxStatus.Pending);
        var res = await _outbox.Find(filter).ToListAsync(cancellationToken: cancellationToken);

        return res ?? new List<Outbox>();
    }

    public async Task<Guid> AddOutboxAsync(Outbox outbox, CancellationToken cancellationToken)
    {
        await _outbox.InsertOneAsync(outbox, cancellationToken: cancellationToken);

        return outbox.Id;
    }
    
    public async Task MarkProcessedAsync(Outbox outbox, CancellationToken cancellationToken)
    {
        var update = Builders<Outbox>.Update
            .Set(o => o.Status, outbox.Status)
            .Set(o => o.UpdatedAt, outbox.UpdatedAt)
            .Set(o => o.CompletedAt, outbox.CompletedAt);

        await _outbox.UpdateOneAsync(o => o.Id == outbox.Id, update, cancellationToken: cancellationToken);
    }
}

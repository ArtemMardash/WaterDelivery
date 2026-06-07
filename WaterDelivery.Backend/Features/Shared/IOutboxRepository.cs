using WaterDelivery.Backend.Infrastructure.Persistence.DbEntities.TransactionOutbox;

namespace WaterDelivery.Backend.Features.Shared;

public interface IOutboxRepository
{
    public Task<List<Outbox>> GetUnprocessedOutboxes(CancellationToken cancellationToken);

    public Task<Guid> AddOutboxAsync(Outbox outbox, CancellationToken cancellationToken);

    public Task MarkProcessedAsync(Outbox outbox, CancellationToken cancellationToken);
}
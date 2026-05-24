namespace WaterDelivery.Backend.Infrastructure.Persistence.DbEntities.TransactionOutbox;

public class Outbox
{
    public Guid Id { get; set; }

    public OutboxStatus Status { get; set; }

    public string PayLoad { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    
    
    public DateTime? CompletedAt { get; set; }
}

public enum OutboxStatus
{
    Unknown = 0,
    Pending, 
    Processed
}
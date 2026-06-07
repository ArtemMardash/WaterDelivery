using WaterDelivery.Contracts.Enums;

namespace WaterDelivery.Backend.Core.Entities;

public class Bill
{
    public Guid Id { get; set; }
    
    public Order Order { get; set; }
    
    public DateTime CreationDate { get; set; }
    
    public DateTime? PaymentDate { get; set; }
    
    public BillStatus Status { get; set; }

    public Bill(Guid id, Order order, DateTime creationDate, DateTime? paymentDate, BillStatus status)
    {
        Id = id;
        Order = order;
        CreationDate = creationDate;
        PaymentDate = paymentDate;
        SetStatus(status);
    }

    public Bill(Order order, DateTime creationDate, DateTime? paymentDate, BillStatus status)
    {
        Id = Guid.NewGuid();
        Order = order;
        SetCreationDate(creationDate);
        SetPaymentDate(paymentDate);
        SetStatus(status);
    }

    public void SetCreationDate(DateTime creation)
    {
        // Normalise to UTC before comparing. The previous version compared the incoming value
        // (sent as DateTime.UtcNow by callers) against DateTime.Now (local), which threw on any
        // machine whose local time is ahead of UTC. Comparing both sides in UTC fixes that.
        var creationUtc = creation.Kind == DateTimeKind.Utc ? creation : creation.ToUniversalTime();

        if (creationUtc < DateTime.UtcNow.AddSeconds(-10))
        {
            throw new ArgumentException($"Creation date can not be less then {DateTime.UtcNow}");
        }

        CreationDate = creation;
    }

    public void SetPaymentDate(DateTime? paymentDate)
    {
        if (paymentDate == null)
        {
            PaymentDate = null;
            return;
        }
        if (paymentDate < CreationDate)
        {
            throw new ArgumentException("Payment date can not be less then Creation date");
        }

        PaymentDate = paymentDate;
    }

    public void SetStatus(BillStatus newStatus)
    {
        switch (Status)
        {
            case BillStatus.Unknown:
                if (newStatus == BillStatus.Unknown)
                    throw new InvalidOperationException("Bill status cannot be set to Unknown");
                Status = newStatus;
                break;
            case BillStatus.WaitForPayment when newStatus is BillStatus.Paid:
                Status = newStatus;
                break;
            case BillStatus.WaitForPayment when newStatus is BillStatus.Cancelled:
                Status = newStatus;
                break;
            default:
                throw new InvalidOperationException($"{Status} can not be changed to {newStatus}");
        }
    }
}
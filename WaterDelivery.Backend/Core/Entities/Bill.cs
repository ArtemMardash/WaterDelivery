using WaterDelivery.Backend.Core.Enums;

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
        if (creation < DateTime.Now)
        {
            throw new ArgumentException($"Creation date can not be less then {DateTime.Now}");
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
                Status = newStatus;
                break;
            case BillStatus.WaitForPayment when newStatus is BillStatus.Paid:
                Status = newStatus;
                break;
            default:
                throw new InvalidOperationException($"{Status} can not be changed to {newStatus}");
        }
    }
}
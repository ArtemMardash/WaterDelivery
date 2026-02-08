using WaterDelivery.Backend.Core.Enums;

namespace WaterDelivery.Backend.Core.Entities;

public class Delivery
{
    public Guid Id { get; set; }
    
    public Guid DeliveryManId { get; set; }
    
    public Order Order { get; set; }
    
    public Address Address { get; set; }
    
    public DeliveryStatus Status { get; set; }

    public Delivery(Guid id, Guid deliveryManId, Order order, Address address, DeliveryStatus status)
    {
        Id = id;
        DeliveryManId = deliveryManId;
        Order = order ?? throw new ArgumentNullException(nameof(order), "Invalid order");
        Address = address ?? throw new ArgumentNullException(nameof(address), "Invalid address");
        SetStatus(status);
    }

    public Delivery(Guid deliveryManId, Order order, Address address, DeliveryStatus status)
    {
        Id = Guid.NewGuid();
        DeliveryManId = deliveryManId;
        Order = order ?? throw new ArgumentNullException(nameof(order), "Invalid order");
        Address = address ?? throw new ArgumentNullException(nameof(address), "Invalid address");
        SetStatus(status);
    }

    public void SetStatus(DeliveryStatus newStatus)
    {
        switch (Status)
        {
            case DeliveryStatus.Unknown:
                Status = newStatus;
                break;
            case DeliveryStatus.Assembly
                when newStatus is DeliveryStatus.TransferredDeliveryService or DeliveryStatus.Cancelled:
                Status = newStatus;
                break;
            case DeliveryStatus.TransferredDeliveryService
                when newStatus is DeliveryStatus.WaitToDelivery or DeliveryStatus.Cancelled:
                Status = newStatus;
                break;
            case DeliveryStatus.WaitToDelivery when newStatus is DeliveryStatus.Delivering or DeliveryStatus.Cancelled:
                Status = newStatus;
                break;
            case DeliveryStatus.Delivering when newStatus is DeliveryStatus.IssuedToCourier:
                Status = newStatus;
                break;
            case DeliveryStatus.IssuedToCourier when newStatus is DeliveryStatus.Delivered or DeliveryStatus.Rejected:
                Status = newStatus;
                break;
            default:
                throw new InvalidOperationException($"{Status} can not be changed to {newStatus}");
        }
    }
}
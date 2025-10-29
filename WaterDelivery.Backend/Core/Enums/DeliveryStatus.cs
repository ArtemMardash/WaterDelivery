namespace WaterDelivery.Backend.Core.Enums;

public enum DeliveryStatus
{
    Unknown = 0,
    Assembly = 1,
    TransferredDeliveryService = 2,
    WaitToDelivery = 3, 
    Delivering = 4,
    IssuedToCourier = 5,
    Delivered = 6,
    Rejected = 7,
    Cancelled = 8
}
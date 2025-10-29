namespace WaterDelivery.Backend.Infrastructure.Persistence.DbEntities;

public class OrderItemDb
{
    public ProductDb Product { get; set; }

    public int Quantity { get; set; }

    public ProductUnitDb ProductUnit { get; set; }
}
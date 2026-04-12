using WaterDelivery.Contracts.Products.Dtos;
using WaterDelivery.Contracts.ProductUnits.Dtos;

namespace WaterDelivery.Contracts.Orders.Dtos;

public class OrderItemDto
{
    public ProductDto Product { get; set; }

    public int Quantity { get; set; }

    public ProductUnitDto ProductUnit { get; set; }
}
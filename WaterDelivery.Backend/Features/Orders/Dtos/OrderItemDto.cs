using WaterDelivery.Backend.Features.Products.Dtos;
using WaterDelivery.Backend.Features.ProductUnits.Dtos;

namespace WaterDelivery.Backend.Features.Orders.Dtos;

public class OrderItemDto
{
    public ProductDto Product { get; set; }

    public int Quantity { get; set; }

    public ProductUnitDto ProductUnit { get; set; }
}
using WaterDelivery.Contracts.ProductUnits.Dtos;

namespace WaterDelivery.Contracts.Products.Dtos;

public class ProductDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    /// <summary>
    /// Options for sale
    /// </summary>
    public List<ProductUnitDto> ProductOptions { get; set; }

    public ProductUnitDto DefaultUnit { get; set; }

    public decimal DefaultUnitPrice { get; set; }
    
    public List<string> ImageLinks { get; set; }
}
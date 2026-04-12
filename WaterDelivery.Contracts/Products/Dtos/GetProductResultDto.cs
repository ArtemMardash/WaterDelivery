using Mediator;
using WaterDelivery.Contracts.ProductUnits.Dtos;

namespace WaterDelivery.Contracts.Products.Dtos;

public class GetProductResultDto: IRequest
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
}
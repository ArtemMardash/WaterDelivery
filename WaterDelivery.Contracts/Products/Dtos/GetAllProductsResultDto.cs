using Mediator;

namespace WaterDelivery.Contracts.Products.Dtos;

public class GetAllProductsResultDto: IRequest
{
    public List<ProductDto> Products { get; set; }
}
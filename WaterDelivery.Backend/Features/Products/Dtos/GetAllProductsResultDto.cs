using Mediator;

namespace WaterDelivery.Backend.Features.Products.Dtos;

public class GetAllProductsResultDto: IRequest
{
    public List<ProductDto> Products { get; set; }
}
using Mediator;
using WaterDelivery.Backend.Features.Products.Dtos;
using WaterDelivery.Backend.Features.Shared;

namespace WaterDelivery.Backend.Features.Products;

public class GetAllProductsUseCase: IRequestHandler<GetAllProductsDto, GetAllProductsResultDto>
{
    private readonly IProductRepository _productRepository;

    public GetAllProductsUseCase(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    
    public async ValueTask<GetAllProductsResultDto> Handle(GetAllProductsDto request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAllProductsAsync(cancellationToken);

        var result = new GetAllProductsResultDto
        {
            Products = products.Select(p => p.ToDto()).ToList()
        };

        return result;
    }
}
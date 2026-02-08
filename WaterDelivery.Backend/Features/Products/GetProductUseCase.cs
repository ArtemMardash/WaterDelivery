using Mediator;
using WaterDelivery.Backend.Features.Products.Dtos;
using WaterDelivery.Backend.Features.Shared;

namespace WaterDelivery.Backend.Features.Products;

public class GetProductUseCase: IRequestHandler<GetProductDto, GetProductResultDto>
{
    private readonly IProductRepository _productRepository;

    public GetProductUseCase(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    
    public async ValueTask<GetProductResultDto> Handle(GetProductDto request, CancellationToken cancellationToken)
    {
        var result = await _productRepository.GetProductByIdAsync(request.Id, cancellationToken);

        return new GetProductResultDto
        {
            Id = result.Id,
            Name = result.Name,
            Description = result.Description,
            ProductOptions = result.ProductOptions.Select(pu=>pu.ToDto()).ToList(),
            DefaultUnit = result.DefaultUnit.ToDto(),
            DefaultUnitPrice = result.DefaultUnitPrice
        };
    }
}
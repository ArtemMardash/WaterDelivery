using Mediator;
using WaterDelivery.Backend.Features.ProductUnits.Dtos;
using WaterDelivery.Backend.Features.Shared;

namespace WaterDelivery.Backend.Features.ProductUnits;

public class GetProductUnitUseCase: IRequestHandler<GetProductUnitDto, GetProductUnitResultDto>
{
    private readonly IProductUnitRepository _productUnitRepository;

    public GetProductUnitUseCase(IProductUnitRepository productUnitRepository)
    {
        _productUnitRepository = productUnitRepository;
    }
    
    public async ValueTask<GetProductUnitResultDto> Handle(GetProductUnitDto request, CancellationToken cancellationToken)
    {
        var result = await _productUnitRepository.GetProductUnitAsync(request.Id, cancellationToken);
        return new GetProductUnitResultDto
        {
            Id = result.Id,
            Name = result.Name,
            QuantityPerUnit = result.QuantityPerUnit
        };
    }
}
using Mediator;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Contracts.ProductUnits.Dtos;

namespace WaterDelivery.Backend.Features.ProductUnits;

public class CreateProductUnitUseCase: IRequestHandler<CreateProductUnitDto, Guid>
{
    private readonly IProductUnitRepository _productUnitRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductUnitUseCase(IProductUnitRepository productUnitRepository, IUnitOfWork unitOfWork)
    {
        _productUnitRepository = productUnitRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async ValueTask<Guid> Handle(CreateProductUnitDto request, CancellationToken cancellationToken)
    {
        var productUnit = new ProductUnit(request.Name, request.QuantityPerUnit);
        var result = await _productUnitRepository.CreateProductUnitAsync(productUnit, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return productUnit.Id;
    }
}
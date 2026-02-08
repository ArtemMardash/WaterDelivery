using Mediator;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.ProductUnits.Dtos;
using WaterDelivery.Backend.Features.Shared;

namespace WaterDelivery.Backend.Features.ProductUnits;

public class UpdateProductUnitUseCase: IRequestHandler<UpdateProductUnitDto>
{
    private readonly IProductUnitRepository _productUnitRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductUnitUseCase(IProductUnitRepository productUnitRepository, IUnitOfWork unitOfWork)
    {
        _productUnitRepository = productUnitRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async ValueTask<Unit> Handle(UpdateProductUnitDto request, CancellationToken cancellationToken)
    {
        var productUnitToUpdate = new ProductUnit(request.Id, request.Name, request.QuantityPerUnit);

        await _productUnitRepository.UpdateProductUnitAsync(productUnitToUpdate, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}
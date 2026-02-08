using Mediator;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.ProductUnits.Dtos;
using WaterDelivery.Backend.Features.Shared;

namespace WaterDelivery.Backend.Features.ProductUnits;

public class DeleteProductUnitUseCase: IRequestHandler<DeleteProductUnitDto>
{
    private readonly IProductUnitRepository _productUnitRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductUnitUseCase(IProductUnitRepository productUnitRepository, IUnitOfWork unitOfWork)
    {
        _productUnitRepository = productUnitRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async ValueTask<Unit> Handle(DeleteProductUnitDto request, CancellationToken cancellationToken)
    {
        await _productUnitRepository.DeleteProductUnitAsync(request.Id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
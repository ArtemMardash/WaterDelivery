using Mediator;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.Products.Dtos;
using WaterDelivery.Backend.Features.Shared;

namespace WaterDelivery.Backend.Features.Products;

public class DeleteProductUseCase: IRequestHandler<DeleteProductDto>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductUseCase(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async ValueTask<Unit> Handle(DeleteProductDto request, CancellationToken cancellationToken)
    {
        await _productRepository.DeleteProductAsync(request.Id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
using Mediator;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.Products.Dtos;
using WaterDelivery.Backend.Features.Shared;

namespace WaterDelivery.Backend.Features.Products;

public class UpdateProductUseCase : IRequestHandler<UpdateProductDto>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductUseCase(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async ValueTask<Unit> Handle(UpdateProductDto request, CancellationToken cancellationToken)
    {
        var productToUpdate = new Product(
            request.Id, 
            request.Name, 
            request.Description,
            request.ProductOptions.Select(pu => pu.ToEntity()).ToList(),
            request.DefaultUnit.ToEntity(), 
            request.DefaultUnitPrice);
        await _productRepository.UpdateProductAsync(productToUpdate, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}
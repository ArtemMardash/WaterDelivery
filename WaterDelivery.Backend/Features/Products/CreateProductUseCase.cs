using Mediator;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.Products.Dtos;
using WaterDelivery.Backend.Features.Shared;

namespace WaterDelivery.Backend.Features.Products;

public class CreateProductUseCase: IRequestHandler<CreateProductDto, Guid>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductUseCase(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async ValueTask<Guid> Handle(CreateProductDto request, CancellationToken cancellationToken)
    {
        var product = new Product(
            request.Name, 
            request.Description, 
            request.ProductOptions.Select(pu=>pu.ToEntity()).ToList(), 
            request.DefaultUnit.ToEntity(),
            request.DefaultUnitPrice);
        var result = await _productRepository.CreateProductAsync(product, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return product.Id;

    }
}
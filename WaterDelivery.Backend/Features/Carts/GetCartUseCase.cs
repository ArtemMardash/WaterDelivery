using Mediator;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Contracts.Carts.Dtos;

namespace WaterDelivery.Backend.Features.Carts;

public class GetCartUseCase: IRequestHandler<GetCartDto, GetCartResultDto>
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public GetCartUseCase(ICartRepository cartRepository, IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }
    public async ValueTask<GetCartResultDto> Handle(GetCartDto request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetCartAsync(request.CustomerId, cancellationToken);
        
        var ids = cart.Items.Keys.ToList();
        var products = await _productRepository.GetProductsAsync(ids, cancellationToken);

        var itemsPrices = products.ToDictionary(p => p.Id,p => p.DefaultUnitPrice);
        return new GetCartResultDto
        {
            CustomerId = cart.CustomerId,
            Items = cart.Items,
            TotalPrice = cart.TotalPrice(itemsPrices)
        };
    }
}
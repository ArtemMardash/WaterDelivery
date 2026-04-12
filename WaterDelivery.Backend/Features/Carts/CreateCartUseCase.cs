using Mediator;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Contracts.Carts.Dtos;

namespace WaterDelivery.Backend.Features.Carts;

public class CreateCartUseCase: IRequestHandler<CreateCartDto, Guid>
{
    private readonly ICartRepository _cartRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCartUseCase(ICartRepository cartRepository, IUnitOfWork unitOfWork)
    {
        _cartRepository = cartRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async ValueTask<Guid> Handle(CreateCartDto request, CancellationToken cancellationToken)
    {
        var cart = new Cart(request.CustomerId, request.Items);
        var res = await _cartRepository.CreateCartAsync(cart, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return res;
    }
}
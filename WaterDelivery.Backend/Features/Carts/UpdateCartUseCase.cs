using Mediator;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Contracts.Carts.Dtos;

namespace WaterDelivery.Backend.Features.Carts;

public class UpdateCartUseCase: IRequestHandler<UpdateCartDto>
{
    private readonly ICartRepository _cartRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCartUseCase(ICartRepository cartRepository, IUnitOfWork unitOfWork)
    {
        _cartRepository = cartRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async ValueTask<Unit> Handle(UpdateCartDto request, CancellationToken cancellationToken)
    {
        var updatedCart = new Cart(request.CustomerId, request.Items);
        await _cartRepository.UpdateCartAsync(updatedCart, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
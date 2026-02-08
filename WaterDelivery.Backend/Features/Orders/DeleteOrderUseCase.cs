using Mediator;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.Orders.Dtos;
using WaterDelivery.Backend.Features.Shared;

namespace WaterDelivery.Backend.Features.Orders;

public class DeleteOrderUseCase: IRequestHandler<DeleteOrderDto>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteOrderUseCase(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async ValueTask<Unit> Handle(DeleteOrderDto request, CancellationToken cancellationToken)
    {
        await _orderRepository.DeleteOrderAsync(request.Id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
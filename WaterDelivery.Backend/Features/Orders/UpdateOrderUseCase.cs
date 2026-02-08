using Mediator;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.Orders.Dtos;
using WaterDelivery.Backend.Features.Shared;

namespace WaterDelivery.Backend.Features.Orders;

public class UpdateOrderUseCase: IRequestHandler<UpdateOrderDto>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateOrderUseCase(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async ValueTask<Unit> Handle(UpdateOrderDto request, CancellationToken cancellationToken)
    {
        var orderToUpdate = new Order(request.Id, request.CustomerId ,request.Items.Select(oi => oi.ToEntity()).ToList());

        await _orderRepository.UpdateOrderAsync(orderToUpdate, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
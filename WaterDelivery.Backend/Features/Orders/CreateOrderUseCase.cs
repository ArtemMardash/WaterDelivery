using Mediator;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.Orders.Dtos;
using WaterDelivery.Backend.Features.Shared;

namespace WaterDelivery.Backend.Features.Orders;

public class CreateOrderUseCase: IRequestHandler<CreateOrderDto, Guid>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateOrderUseCase(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }
    
    
    public async ValueTask<Guid> Handle(CreateOrderDto request, CancellationToken cancellationToken)
    {
        //TODO проверить Items на Null and empty
        var items = request.Items.Select(oi => oi.ToEntity()).ToList();
        var order = new Order(request.CustomerId, items);
        var result = await _orderRepository.CreateOrderAsync(order, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return order.Id;
    }
    
}
using Mediator;
using WaterDelivery.Backend.Features.Orders.Dtos;
using WaterDelivery.Backend.Features.Shared;

namespace WaterDelivery.Backend.Features.Orders;

public class GetOrderUseCase: IRequestHandler<GetOrderDto, GetOrderResultDto>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderUseCase(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    
    public async ValueTask<GetOrderResultDto> Handle(GetOrderDto request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetOrderByIdAsync(request.Id, cancellationToken);

        return new GetOrderResultDto
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            Items = order.Items.Select(oi => oi.ToDto()).ToList()
        };
    }
}
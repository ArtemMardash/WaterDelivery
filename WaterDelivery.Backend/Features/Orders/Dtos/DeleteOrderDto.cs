using Mediator;

namespace WaterDelivery.Backend.Features.Orders.Dtos;

public class DeleteOrderDto: IRequest
{
    public Guid Id { get; set; }
}
using Mediator;

namespace WaterDelivery.Contracts.Orders.Dtos;

public class DeleteOrderDto: IRequest
{
    public Guid Id { get; set; }
}
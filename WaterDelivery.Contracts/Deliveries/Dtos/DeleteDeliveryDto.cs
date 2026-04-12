using Mediator;

namespace WaterDelivery.Contracts.Deliveries.Dtos;

public class DeleteDeliveryDto: IRequest
{
    public Guid Id { get; set; }
}
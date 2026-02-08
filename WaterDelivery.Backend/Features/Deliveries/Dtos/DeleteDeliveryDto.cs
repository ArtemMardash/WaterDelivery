using Mediator;

namespace WaterDelivery.Backend.Features.Deliveries.Dtos;

public class DeleteDeliveryDto: IRequest
{
    public Guid Id { get; set; }
}
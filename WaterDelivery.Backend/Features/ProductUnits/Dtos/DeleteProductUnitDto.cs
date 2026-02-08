using Mediator;

namespace WaterDelivery.Backend.Features.ProductUnits.Dtos;

public class DeleteProductUnitDto: IRequest
{
    public Guid Id { get; set; }

}
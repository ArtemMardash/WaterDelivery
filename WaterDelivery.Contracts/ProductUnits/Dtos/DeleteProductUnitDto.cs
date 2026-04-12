using Mediator;

namespace WaterDelivery.Contracts.ProductUnits.Dtos;

public class DeleteProductUnitDto: IRequest
{
    public Guid Id { get; set; }

}
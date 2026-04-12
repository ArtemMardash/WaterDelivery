using Mediator;
using WaterDelivery.Contracts.Enums;

namespace WaterDelivery.Contracts.ProductUnits.Dtos;

public class CreateProductUnitDto: IRequest<Guid>
{
    public MeasurementUnits Name { get; set; }

    public int QuantityPerUnit { get; set; }
}
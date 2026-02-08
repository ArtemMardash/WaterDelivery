using Mediator;
using WaterDelivery.Backend.Core.Enums;

namespace WaterDelivery.Backend.Features.ProductUnits.Dtos;

public class CreateProductUnitDto: IRequest<Guid>
{
    public MeasurementUnits Name { get; set; }

    public int QuantityPerUnit { get; set; }
}
using WaterDelivery.Backend.Core.Enums;

namespace WaterDelivery.Backend.Features.ProductUnits.Dtos;

public class ProductUnitDto
{
    public Guid Id { get; set; }

    public MeasurementUnits Name { get; set; }

    public int QuantityPerUnit { get; set; }
}
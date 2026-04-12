using WaterDelivery.Contracts.Enums;

namespace WaterDelivery.Contracts.ProductUnits.Dtos;

public class GetProductUnitResultDto
{
    public Guid Id { get; set; }

    public MeasurementUnits Name { get; set; }

    public int QuantityPerUnit { get; set; }
}
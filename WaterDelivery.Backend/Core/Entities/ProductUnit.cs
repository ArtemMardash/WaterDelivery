using WaterDelivery.Backend.Core.Enums;

namespace WaterDelivery.Backend.Core.Entities;

public class ProductUnit
{
    public Guid Id { get; set; }

    public MeasurementUnits Name { get; set; }

    public int QuantityPerUnit { get; set; }

    public ProductUnit(MeasurementUnits name, int quantityPerUnit)
    {
        Id = Guid.NewGuid();
        Name = name;
        QuantityPerUnit = quantityPerUnit <= 0
            ? throw new ArgumentException("Quantity per unit cannot be less then 0", nameof(QuantityPerUnit))
            : quantityPerUnit;
    }

    public ProductUnit(Guid id, MeasurementUnits name, int quantityPerUnit)
    {
        Id = id;
        Name = name;
        QuantityPerUnit = quantityPerUnit <= 0
            ? throw new ArgumentException("Quantity per unit cannot be less then 0", nameof(QuantityPerUnit))
            : quantityPerUnit;
    }
}
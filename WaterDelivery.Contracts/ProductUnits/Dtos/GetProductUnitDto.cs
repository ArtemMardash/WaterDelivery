using Mediator;

namespace WaterDelivery.Contracts.ProductUnits.Dtos;

public class GetProductUnitDto: IRequest<GetProductUnitResultDto>
{
    public Guid Id { get; set; }

}
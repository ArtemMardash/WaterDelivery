using Mediator;

namespace WaterDelivery.Backend.Features.ProductUnits.Dtos;

public class GetProductUnitDto: IRequest<GetProductUnitResultDto>
{
    public Guid Id { get; set; }

}
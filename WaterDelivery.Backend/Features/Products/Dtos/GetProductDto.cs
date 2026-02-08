using Mediator;

namespace WaterDelivery.Backend.Features.Products.Dtos;

public class GetProductDto: IRequest<GetProductResultDto>
{
    public Guid Id { get; set; }
}
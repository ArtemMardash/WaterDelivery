using Mediator;

namespace WaterDelivery.Contracts.Products.Dtos;

public class GetProductDto: IRequest<GetProductResultDto>
{
    public Guid Id { get; set; }
}
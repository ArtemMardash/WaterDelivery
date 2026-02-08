using Mediator;

namespace WaterDelivery.Backend.Features.Products.Dtos;

public class DeleteProductDto: IRequest
{
    public Guid Id { get; set; }

}
using Mediator;

namespace WaterDelivery.Contracts.Products.Dtos;

public class DeleteProductDto: IRequest
{
    public Guid Id { get; set; }

}
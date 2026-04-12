using Mediator;

namespace WaterDelivery.Contracts.Carts.Dtos;

public class GetCartDto: IRequest<GetCartResultDto>
{
    public Guid CustomerId;
}
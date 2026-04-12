using Mediator;

namespace WaterDelivery.Contracts.Users.Dtos;

public class GetUserDto: IRequest<GetUserResultDto>
{
    public Guid Id { get; set; }
}
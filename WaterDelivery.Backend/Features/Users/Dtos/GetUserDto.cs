using Mediator;

namespace WaterDelivery.Backend.Features.Users.Dtos;

public class GetUserDto: IRequest<GetUserResultDto>
{
    public Guid Id { get; set; }
}
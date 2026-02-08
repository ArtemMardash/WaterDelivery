using Mediator;

namespace WaterDelivery.Backend.Features.Users.Dtos;

public class DeleteUserDto : IRequest
{
    public Guid Id { get; set; }
}
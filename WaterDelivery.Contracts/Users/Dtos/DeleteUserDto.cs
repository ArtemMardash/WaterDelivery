using Mediator;

namespace WaterDelivery.Contracts.Users.Dtos;

public class DeleteUserDto : IRequest
{
    public Guid Id { get; set; }
}
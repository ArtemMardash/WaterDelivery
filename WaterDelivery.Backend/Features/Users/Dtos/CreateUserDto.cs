using Mediator;
using WaterDelivery.Backend.Core.Enums;

namespace WaterDelivery.Backend.Features.Users.Dtos;

public class CreateUserDto: IRequest<Guid>
{
    public string Name { get; set; }
    
    public UserType UserType { get; set; }
    
    public string Email { get; set; }
    
    public string PhoneNumber { get; set; }
}
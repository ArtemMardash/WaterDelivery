using Mediator;
using WaterDelivery.Contracts.Enums;

namespace WaterDelivery.Contracts.Users.Dtos;

public class UpdateUserDto : IRequest
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public UserType UserType { get; set; }
    
    public string Email { get; set; }
    
    public string PhoneNumber { get; set; }
}
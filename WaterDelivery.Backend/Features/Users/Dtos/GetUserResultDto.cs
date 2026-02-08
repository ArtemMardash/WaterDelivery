using WaterDelivery.Backend.Core.Enums;

namespace WaterDelivery.Backend.Features.Users.Dtos;

public class GetUserResultDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public UserType UserType { get; set; }
    
    public string Email { get; set; }
    
    public string PhoneNumber { get; set; }
}
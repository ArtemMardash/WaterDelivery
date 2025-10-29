using WaterDelivery.Backend.Core.Enums;

namespace WaterDelivery.Backend.Features;

public class CreateUserDto
{
    public string Name { get; set; }
    
    public UserType UserType { get; set; }
}
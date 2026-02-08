using WaterDelivery.Backend.Core.Enums;
using WaterDelivery.Backend.Infrastructure.Extensions;

namespace WaterDelivery.Backend.Core.Entities;

public class User
{
    private const int NAME_MIN_LENGTH = 5;
    private const int NAME_MAX_LENGTH = 40;

    public Guid Id { get; set; }

    public string Name { get; set; }

    public UserType UserType { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public User(Guid id, string name, UserType userType, string email, string phoneNumber)
    {
        Id = id;
        SetName(name);
        UserType = userType == UserType.Unknown
            ? throw new ArgumentException("User type can not be unknown", nameof(userType))
            : userType;
        Email = email.IsValidEmail() 
            ? email 
            : throw new ArgumentException("Invalid Email", nameof(email));
        PhoneNumber = phoneNumber.IsValidPhoneNumber()
            ? phoneNumber
            : throw new ArgumentException("Invalid phone number", nameof(phoneNumber));
    }

    public User(string name, UserType userType, string email, string phoneNumber)
    {
        Id = Guid.NewGuid();
        SetName(name);
        UserType = userType == UserType.Unknown
            ? throw new ArgumentException("User type can not be unknown", nameof(userType))
            : userType;
        Email = email.IsValidEmail() 
            ? email 
            : throw new ArgumentException("Invalid Email", nameof(email));
        PhoneNumber = phoneNumber.IsValidPhoneNumber()
            ? phoneNumber
            : throw new ArgumentException("Invalid phone number", nameof(phoneNumber));
    }

    private void SetName(string input)
    {
        if (input.Length < NAME_MIN_LENGTH || input.Length > NAME_MAX_LENGTH)
        {
            throw new ArgumentException(
                $"Name should be longer then {NAME_MIN_LENGTH} and shorter then {NAME_MAX_LENGTH}", nameof(Name));
        }

        Name = input;
    }
}
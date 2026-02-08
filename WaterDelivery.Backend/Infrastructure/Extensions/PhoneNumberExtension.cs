using System.Text.RegularExpressions;

namespace WaterDelivery.Backend.Infrastructure.Extensions;

public static class PhoneNumberExtension
{
    /// <summary>
    /// check if phone number is valid and suit for regular expression
    /// </summary>
    public static bool IsValidPhoneNumber(this string phoneNumber)
    {
        if (string.IsNullOrEmpty(phoneNumber) || phoneNumber.Length > 14)
        {
            return false;
        }
        
        try
        {
            return Regex.IsMatch(phoneNumber, @"^1?[-.\s]?\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4}$", RegexOptions.IgnoreCase,
                TimeSpan.FromMilliseconds(250));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }
}
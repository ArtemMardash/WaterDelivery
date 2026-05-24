using Mediator;

namespace WaterDelivery.Contracts.GoogleAuth.Dtos;

public class GoogleLoginRequest : IRequest<GoogleLoginResult>
{
    public string GoogleId { get; set; }
    
    public string Email { get; set; }

    public string? AccessToken { get; set; }

    public string? RefreshToken { get; set; }   

    public DateTime? Expiry { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }
}

public class GoogleLoginResult
{
    public  Guid UserId { get; set; }
    /// <summary>
    /// AccessToken
    /// </summary>
    public string AccessToken { get; set; }

    /// <summary>
    /// RefreshToken 
    /// </summary>
    public string RefreshToken { get; set; }
}
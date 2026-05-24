using Mediator;
using Microsoft.AspNetCore.Identity;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Backend.Infrastructure.Persistence.DbEntities;
using WaterDelivery.Contracts.Enums;
using WaterDelivery.Contracts.GoogleAuth.Dtos;

namespace WaterDelivery.Backend.Features.GoogleAuth;

public class GoogleLoginUseCase : IRequestHandler<GoogleLoginRequest, GoogleLoginResult>
{
    /// <summary>
    /// User repository to get user
    /// </summary>
    private readonly IUserRepository _userRepository;

    private readonly UserManager<UserDb> _userManager;

    public GoogleLoginUseCase(IUserRepository userRepository, UserManager<UserDb> userManager)
    {
        _userRepository = userRepository;
        _userManager = userManager;
    }

    public async ValueTask<GoogleLoginResult> Handle(GoogleLoginRequest request, CancellationToken cancellationToken)
    {
        UserDb? user;
        var isNewUser = false;
        user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            var id = Guid.NewGuid();
            user = new UserDb
            {
                Uid = id,
                UserName = request.Email,
                EmailConfirmed = true,
                Name = $"{request.LastName} {request.FirstName}",
                UserType = (int) UserType.Customer,
                Email = request.Email,
                GoogleId = request.GoogleId,
                GoogleRefreshToken = request.RefreshToken,
                GoogleRefreshTokenExpiry = request.Expiry,
                GoogleAccessToken = request.AccessToken,
            };

            var result = await _userManager.CreateAsync(user);
            if (result.Errors.Any())
            {
                throw new InvalidOperationException(string.Join(',', result.Errors));
            }
        }
        return new GoogleLoginResult
        {
            UserId = user.Uid,
            AccessToken = request.AccessToken,
            RefreshToken = request.RefreshToken
        };
    }
}
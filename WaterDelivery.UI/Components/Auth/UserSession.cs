using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace WaterDelivery.UI.Components.Auth;

/// <summary>
/// Thin wrapper over the authentication cookie. The signed-in user id is read from the
/// ClaimsPrincipal (NameIdentifier claim), so it survives page refreshes and is consistent
/// across every component - unlike the previous in-memory-only version.
/// </summary>
public class UserSession
{
    private readonly AuthenticationStateProvider _authStateProvider;

    public UserSession(AuthenticationStateProvider authStateProvider)
    {
        _authStateProvider = authStateProvider;
    }

    public async Task<Guid?> GetUserIdAsync()
    {
        var state = await _authStateProvider.GetAuthenticationStateAsync();
        var idValue = state.User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(idValue, out var id) ? id : null;
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var id = await GetUserIdAsync();
        return id is not null;
    }
}

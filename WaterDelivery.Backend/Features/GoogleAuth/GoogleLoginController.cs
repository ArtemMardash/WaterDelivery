using System.Security.Claims;
using Mediator;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using WaterDelivery.Backend.Features.GoogleAuth.Dtos;

namespace WaterDelivery.Backend.Features.GoogleAuth;

public static class GoogleLoginController
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
            var group = app.MapGroup("/api/auth")
                .WithTags("Google");

        group.MapGet("/signin", async (HttpContext context, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var result = await context.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
                if (!result.Succeeded) return Results.Unauthorized();
                var principal = result.Principal;
                var accessToken = result.Properties.Items[".Token.access_token"];
                _ =
                    result.Properties.Items.TryGetValue(".Token.refresh_token", out var refreshToken);
                _ = DateTime.TryParse(result.Properties.Items[".Token.expires_at"], out var tokenExpiry);

                var request = new GoogleLoginRequest
                {
                    GoogleId = principal.FindFirstValue(ClaimTypes.NameIdentifier),
                    Email = principal.FindFirstValue(ClaimTypes.Email),
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    Expiry = tokenExpiry,
                    FirstName = principal.FindFirstValue(ClaimTypes.Name),
                    LastName = principal.FindFirstValue(ClaimTypes.Surname)
                };
                try
                {
                    var tokens = await mediator.Send(request, cancellationToken);
                        
                    return Results.Redirect("http://localhost:5167/place-order");
                }
                catch (Exception Ex)
                {
                    Console.WriteLine(Ex.Message);
                    return Results.Redirect("http://localhost:5167/Unauthorize");
                }
               

            })
            .WithName("GoogleCallBack")
            .WithOpenApi();

        group.MapGet("/login",
                async (HttpContext context) =>
                {
                    var props = new AuthenticationProperties
                    {
                        RedirectUri = "http://localhost:5017/api/auth/signin"
                    };
                    await context.ChallengeAsync(GoogleDefaults.AuthenticationScheme, props);
                })
            .WithName("GoogleLogin")    
            .WithOpenApi();

        group.MapGet("/google/profile", (ClaimsPrincipal user) => { return user; })
            .RequireAuthorization();
    }
}
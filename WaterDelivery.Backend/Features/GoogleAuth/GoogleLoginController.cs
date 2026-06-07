using Mediator;
using Microsoft.AspNetCore.Mvc;
using WaterDelivery.Contracts.GoogleAuth.Dtos;

namespace WaterDelivery.Backend.Features.GoogleAuth;

public static class GoogleLoginController
{
    /// <summary>
    /// The Google OAuth handshake now lives entirely in the UI project (it owns the cookie and
    /// therefore the session that checkout reads). The backend no longer performs the OAuth
    /// challenge/callback; it just exposes a plain endpoint that upserts the user coming from a
    /// Google profile and hands back the internal user id.
    /// </summary>
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/auth")
            .WithTags("Google");

        group.MapPost("/google-user",
                async ([FromBody] GoogleLoginRequest request, [FromServices] IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    try
                    {
                        var result = await mediator.Send(request, cancellationToken);
                        return Results.Ok(result);
                    }
                    catch (Exception ex)
                    {
                        return Results.BadRequest(ex.Message);
                    }
                })
            .WithName("UpsertGoogleUser")
            .WithOpenApi();
    }
}

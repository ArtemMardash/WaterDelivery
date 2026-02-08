using Mediator;
using Microsoft.AspNetCore.Mvc;
using WaterDelivery.Backend.Features.Users.Dtos;

namespace WaterDelivery.Backend.Features.Users;

public static class UserController
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/waterDelivery/user")
            .WithTags("user");

        group.MapPost("/",
                async ([FromBody] CreateUserDto dto, [FromServices] IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    var result = await mediator.Send(dto, cancellationToken);
                    return result;
                })
            .WithName("CreateUser")
            .WithOpenApi();

        group.MapGet("/{id:guid}", async (Guid id, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var dto = new GetUserDto { Id = id };
                var result = await mediator.Send(dto, cancellationToken);
                return result;
            })
            .WithName("GetUser")
            .WithOpenApi();

        group.MapPut("/",
                async ([FromBody] UpdateUserDto dto, [FromServices] IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    await mediator.Send(dto, cancellationToken);
                })
            .WithName("UpdateUser")
            .WithOpenApi();

        group.MapDelete("/{id:guid}", async (Guid id, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var dto = new DeleteUserDto { Id = id };
                await mediator.Send(dto, cancellationToken);
            })
            .WithName("DeleteUser")
            .WithOpenApi();
    }
}
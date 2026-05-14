using Mediator;
using Microsoft.AspNetCore.Mvc;
using WaterDelivery.Contracts.Carts.Dtos;

namespace WaterDelivery.Backend.Features.Carts;

public static class CartController
{
    public static void MapCartEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/waterDelivery/Cart")
            .WithTags("Cart");

        group.MapPost("/", async ([FromBody]CreateCartDto dto,[FromServices] IMediator mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(dto, cancellationToken);
                return result;
            })
            .WithName("CreateCart")
            .WithOpenApi();

        group.MapGet("/{id:guid}", async (Guid id, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var dto = new GetCartDto { CustomerId = id };
                var result = await mediator.Send(dto, cancellationToken);
                return result;
            })
            .WithName("GetCart")
            .WithOpenApi();

        group.MapPut("/", async ( [FromBody] UpdateCartDto dto,[FromServices] IMediator mediator, CancellationToken cancellationToken) =>
            {
                await mediator.Send(dto, cancellationToken);
            })
            .WithName("UpdateCart")
            .WithOpenApi();
    }
}
using Mediator;
using Microsoft.AspNetCore.Mvc;
using WaterDelivery.Backend.Features.Orders.Dtos;

namespace WaterDelivery.Backend.Features.Orders;

public static class OrderController
{
    public static void MapOrderEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/waterDelivery/order")
            .WithTags("order");

        group.MapPost("/", async ([FromBody]CreateOrderDto dto,[FromServices] IMediator mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(dto, cancellationToken);
                return result;
            })
            .WithName("CreateOrder")
            .WithOpenApi();

        group.MapGet("/{id:guid}", async (Guid id, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var dto = new GetOrderDto
                {
                    Id = id
                };
                var result = await mediator.Send(dto, cancellationToken);
                return result;
            })
            .WithName("GetOrder")
            .WithOpenApi();

        group.MapPut("/", async ([FromBody] UpdateOrderDto dto,[FromServices] IMediator mediator, CancellationToken cancellationToken) =>
            {
                await mediator.Send(dto, cancellationToken);
            })
            .WithName("UpdateOrder")
            .WithOpenApi();

        group.MapDelete("/{id:guid}", async (Guid id, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var dto = new DeleteOrderDto
                {
                    Id = id
                };
                await mediator.Send(dto, cancellationToken);
            })
            .WithName("DeleteOrder")
            .WithOpenApi();
    }
}
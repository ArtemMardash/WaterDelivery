using Mediator;
using Microsoft.AspNetCore.Mvc;
using WaterDelivery.Backend.Features.Deliveries.Dtos;

namespace WaterDelivery.Backend.Features.Deliveries;

public static class DeliveryController
{
    public static void MapDeliveryEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/waterDelivery/delivery")
            .WithTags("delivery");

        group.MapPost("/", async ([FromBody]CreateDeliveryDto dto,[FromServices] IMediator mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(dto, cancellationToken);
                return result;
            })
            .WithName("CreateDelivery")
            .WithOpenApi();

        group.MapGet("/{id:guid}", async (Guid id, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var dto = new GetDeliveryDto
                {
                    Id = id
                };
                var result = await mediator.Send(dto, cancellationToken);
                return result;
            })
            .WithName("GetDelivery")
            .WithOpenApi();

        group.MapPut("/", async ([FromBody]UpdateDeliveryDto dto,[FromServices] IMediator mediator, CancellationToken cancellationToken) =>
            {
                await mediator.Send(dto, cancellationToken);
            })
            .WithName("UpdateDelivery")
            .WithOpenApi();

        group.MapDelete("/{id:guid}", async (Guid id, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var dto = new DeleteDeliveryDto
                {
                    Id = id
                };
                await mediator.Send(dto, cancellationToken);
            })
            .WithName("DeleteDelivery")
            .WithOpenApi();
    }
}
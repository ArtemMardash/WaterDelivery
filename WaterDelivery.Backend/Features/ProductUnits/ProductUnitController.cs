using Mediator;
using Microsoft.AspNetCore.Mvc;
using WaterDelivery.Backend.Features.ProductUnits.Dtos;

namespace WaterDelivery.Backend.Features.ProductUnits;

public static class ProductUnitController
{
    public static void MapProductUnitEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/waterDelivery/productUnit")
            .WithTags("productUnit");

        group.MapPost("/", async ([FromBody] CreateProductUnitDto dto,[FromServices] IMediator mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(dto, cancellationToken);
                return result;
            })
            .WithName("CreateProductUnit")
            .WithOpenApi();

        group.MapGet("/{id:guid}", async (Guid id, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var dto = new GetProductUnitDto { Id = id };
                var result = await mediator.Send(dto, cancellationToken);
                return result;
            })
            .WithName("GetProductUnit")
            .WithOpenApi();

        group.MapPut("/", async ([FromBody] UpdateProductUnitDto dto,[FromServices] IMediator mediator, CancellationToken cancellationToken) =>
            {
                await mediator.Send(dto, cancellationToken);
            })
            .WithName("UpdateProductUnit")
            .WithOpenApi();

        group.MapDelete("/{id:guid}", async (Guid id, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var dto = new DeleteProductUnitDto { Id = id };
                await mediator.Send(dto, cancellationToken);
            })
            .WithName("DeleteProductUnit")
            .WithOpenApi();
    }
}
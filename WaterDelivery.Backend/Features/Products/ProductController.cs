using Mediator;
using Microsoft.AspNetCore.Mvc;
using WaterDelivery.Backend.Features.Products.Dtos;

namespace WaterDelivery.Backend.Features.Products;

public static class ProductController
{
    public static void MapProductEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/waterDelivery/product")
            .WithTags("product");

        group.MapPost("/", async ([FromBody] CreateProductDto dto,[FromServices] IMediator mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(dto, cancellationToken);
                return result;
            })
            .WithName("CreateProduct")
            .WithOpenApi();

        group.MapGet("/{id:guid}", async (Guid id, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var dto = new GetProductDto
                {
                    Id = id
                };
                var result = await mediator.Send(dto, cancellationToken);
                return result;
            })
            .WithName("GetProduct")
            .WithOpenApi();

        group.MapPut("/", async ([FromBody] UpdateProductDto dto,[FromServices] IMediator mediator, CancellationToken cancellationToken) =>
            {
                await mediator.Send(dto, cancellationToken);
            })
            .WithName("UpdateProduct")
            .WithOpenApi();

        group.MapDelete("/{id:guid}", async (Guid id, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var dto = new DeleteProductDto
                {
                    Id = id
                };
                await mediator.Send(dto, cancellationToken);
            })
            .WithName("DeleteProduct")
            .WithOpenApi();
    }
}
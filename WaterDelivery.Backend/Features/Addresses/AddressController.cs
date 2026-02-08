using Mediator;
using Microsoft.AspNetCore.Mvc;
using WaterDelivery.Backend.Features.Addresses.Dtos;

namespace WaterDelivery.Backend.Features.Addresses;

public static class AddressController
{
    public static void MapAddressEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/waterDelivery/address")
            .WithTags("address");
        
        group.MapPost("/", async ([FromBody]CreateAddressDto dto, [FromServices]IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(dto, cancellationToken);
            return result;
        })
        .WithName("CreateAddress")
        .WithOpenApi();
        
        group.MapGet("/{id:guid}", async (Guid id, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var dto = new GetAddressDto
                {
                    Id = id
                };
                var result = await mediator.Send(dto, cancellationToken);
                return result;
            })
            .WithName("GetAddress")
            .WithOpenApi();
        
        group.MapPut("/", async ([FromBody] UpdateAddressDto dto, [FromServices] IMediator mediator, CancellationToken cancellationToken) =>
            {
                await mediator.Send(dto, cancellationToken);
            })
            .WithName("UpdateAddress")
            .WithOpenApi();
        
        group.MapDelete("/{id:guid}", async (Guid id, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var dto = new DeleteAddressDto
                {
                    Id = id
                };
                await mediator.Send(dto, cancellationToken);
            })
            .WithName("DeleteAddress")
            .WithOpenApi();
    }
}
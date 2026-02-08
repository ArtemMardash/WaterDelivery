using Mediator;
using Microsoft.AspNetCore.Mvc;
using WaterDelivery.Backend.Features.CustomersAddresses.Dtos;

namespace WaterDelivery.Backend.Features.CustomersAddresses;

public static class CustomerAddressController
{
    public static void MapCustomerAddressEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/waterDelivery/customerAddress")
            .WithTags("customerAddress");

        group.MapPost("/", async ([FromBody]CreateCustomerAddressesDto dto,[FromServices] IMediator mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(dto, cancellationToken);
                return result;
            })
            .WithName("CreateCustomerAddress")
            .WithOpenApi();

        group.MapGet("/{id:guid}", async (Guid id, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var dto = new GetCustomerAddressesDto { Id = id };
                var result = await mediator.Send(dto, cancellationToken);
                return result;
            })
            .WithName("GetCustomerAddress")
            .WithOpenApi();

        group.MapPut("/", async ( [FromBody] UpdateCustomerAddressesDto dto,[FromServices] IMediator mediator, CancellationToken cancellationToken) =>
            {
                await mediator.Send(dto, cancellationToken);
            })
            .WithName("UpdateCustomerAddress")
            .WithOpenApi();

        group.MapDelete("/{id:guid}", async (Guid id, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var dto = new DeleteCustomerAddressesDto { Id = id };
                await mediator.Send(dto, cancellationToken);
            })
            .WithName("DeleteCustomerAddress")
            .WithOpenApi();
    }
}
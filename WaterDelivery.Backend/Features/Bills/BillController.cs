using Mediator;
using Microsoft.AspNetCore.Mvc;
using WaterDelivery.Backend.Features.Bills.Dtos;

namespace WaterDelivery.Backend.Features.Bills;

public static class BillController
{
    public static void MapBillEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/waterDelivery/bill")
            .WithTags("bill");

        group.MapPost("/",
                async ([FromBody] CreateBillDto dto, [FromServices] IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    var result = await mediator.Send(dto, cancellationToken);
                    return result;
                })
            .WithName("CreateBill")
            .WithOpenApi();

        group.MapGet("/{id:guid}", async (Guid id, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var dto = new GetBillDto
                {
                    Id = id
                };
                var result = await mediator.Send(dto, cancellationToken);
                return result;
            })
            .WithName("GetBill")
            .WithOpenApi();

        group.MapPut("/",
                async ([FromBody]UpdateBillDto dto,[FromServices] IMediator mediator, CancellationToken cancellationToken) =>
                {
                    await mediator.Send(dto, cancellationToken);
                })
            .WithName("UpdateBill")
            .WithOpenApi();

        group.MapDelete("/{id:guid}",
                async (Guid id, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var dto = new DeleteBillDto
                    {
                        Id = id
                    };
                    await mediator.Send(dto, cancellationToken);
                })
            .WithName("DeleteBill")
            .WithOpenApi();
    }
}
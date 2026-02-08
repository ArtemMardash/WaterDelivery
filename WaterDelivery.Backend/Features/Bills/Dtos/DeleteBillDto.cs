using Mediator;

namespace WaterDelivery.Backend.Features.Bills.Dtos;

public class DeleteBillDto: IRequest
{
    public Guid Id { get; set; }
}
using Mediator;

namespace WaterDelivery.Contracts.Bills.Dtos;

public class DeleteBillDto: IRequest
{
    public Guid Id { get; set; }
}
using Mediator;

namespace WaterDelivery.Contracts.Bills.Dtos;

public class GetBillDto: IRequest<GetBillResultDto>
{
    public Guid Id { get; set; }
}
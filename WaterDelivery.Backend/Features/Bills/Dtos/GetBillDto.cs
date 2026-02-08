using Mediator;

namespace WaterDelivery.Backend.Features.Bills.Dtos;

public class GetBillDto: IRequest<GetBillResultDto>
{
    public Guid Id { get; set; }
}
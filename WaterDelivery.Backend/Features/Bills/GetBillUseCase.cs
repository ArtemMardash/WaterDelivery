using Mediator;
using WaterDelivery.Backend.Features.Bills.Dtos;
using WaterDelivery.Backend.Features.Shared;

namespace WaterDelivery.Backend.Features.Bills;

public class GetBillUseCase: IRequestHandler<GetBillDto, GetBillResultDto>
{
    private readonly IBillRepository _billRepository;

    public GetBillUseCase(IBillRepository billRepository)
    {
        _billRepository = billRepository;
    }
    
    public async ValueTask<GetBillResultDto> Handle(GetBillDto request, CancellationToken cancellationToken)
    {
        var result = await _billRepository.GetBillByIdAsync(request.Id, cancellationToken);

        return new GetBillResultDto
        {
            Id = result.Id,
            Order = result.Order.ToDto(),
            CreationDate = result.CreationDate,
            PaymentDate = result.PaymentDate,
            Status = result.Status
        };
    }
}
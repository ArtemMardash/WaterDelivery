using Mediator;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Contracts.Bills.Dtos;

namespace WaterDelivery.Backend.Features.Bills;

public class CreateBillUseCase: IRequestHandler<CreateBillDto, Guid>
{
    private readonly IBillRepository _billRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBillUseCase(IBillRepository billRepository, IUnitOfWork unitOfWork)
    {
        _billRepository = billRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async ValueTask<Guid> Handle(CreateBillDto request, CancellationToken cancellationToken)
    {
        var bill = new Bill(request.Order.ToEntity(), request.CreationDate, request.PaymentDate, request.Status);

        var result = await _billRepository.CreateBillAsync(bill, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return bill.Id;
    }
}
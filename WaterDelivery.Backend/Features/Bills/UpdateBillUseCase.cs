using Mediator;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.Bills.Dtos;
using WaterDelivery.Backend.Features.Shared;

namespace WaterDelivery.Backend.Features.Bills;

public class UpdateBillUseCase: IRequestHandler<UpdateBillDto>
{
    private readonly IBillRepository _billRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBillUseCase(IBillRepository billRepository, IUnitOfWork unitOfWork)
    {
        _billRepository = billRepository;
        _unitOfWork = unitOfWork;
    }   
    
    public async ValueTask<Unit> Handle(UpdateBillDto request, CancellationToken cancellationToken)
    {
        var billToUpdate = new Bill(request.Id, request.Order.ToEntity(), request.CreationDate, request.PaymentDate,
            request.Status);
        await  _billRepository.UpdateBillAsync(billToUpdate, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
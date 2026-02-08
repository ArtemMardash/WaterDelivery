using Mediator;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.Bills.Dtos;
using WaterDelivery.Backend.Features.Shared;

namespace WaterDelivery.Backend.Features.Bills;

public class DeleteBillUseCase: IRequestHandler<DeleteBillDto>
{
    private readonly IBillRepository _billRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBillUseCase(IBillRepository billRepository, IUnitOfWork unitOfWork)
    {
        _billRepository = billRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async ValueTask<Unit> Handle(DeleteBillDto request, CancellationToken cancellationToken)
    {
        await _billRepository.DeleteAsync(request.Id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}
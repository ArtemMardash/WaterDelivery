using Mediator;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.CustomersAddresses.Dtos;
using WaterDelivery.Backend.Features.Shared;

namespace WaterDelivery.Backend.Features.CustomersAddresses;

public class DeleteCustomerAddressesUseCase: IRequestHandler<DeleteCustomerAddressesDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICustomerAddressesRepository _customerAddressesRepository;

    public DeleteCustomerAddressesUseCase(IUnitOfWork unitOfWork, ICustomerAddressesRepository customerAddressesRepository)
    {
        _unitOfWork = unitOfWork;
        _customerAddressesRepository = customerAddressesRepository;
    }
    
    public async ValueTask<Unit> Handle(DeleteCustomerAddressesDto request, CancellationToken cancellationToken)
    {
        await _customerAddressesRepository.DeleteCustomerAddressesAsync(request.Id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
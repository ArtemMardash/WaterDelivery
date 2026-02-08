using Mediator;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.CustomersAddresses.Dtos;
using WaterDelivery.Backend.Features.Shared;

namespace WaterDelivery.Backend.Features.CustomersAddresses;

public class UpdateCustomerAddressUseCase: IRequestHandler<UpdateCustomerAddressesDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICustomerAddressesRepository _customerAddressesRepository;

    public UpdateCustomerAddressUseCase(IUnitOfWork unitOfWork, ICustomerAddressesRepository customerAddressesRepository)
    {
        _unitOfWork = unitOfWork;
        _customerAddressesRepository = customerAddressesRepository;
    }
    
    public async ValueTask<Unit> Handle(UpdateCustomerAddressesDto request, CancellationToken cancellationToken)
    {
        await _customerAddressesRepository.UpdateCustomerAddressesAsync(
            new CustomerAddresses(request.Id, request.CustomerId, request.Addresses.Select(a => a.ToEntity()).ToList()),
            cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
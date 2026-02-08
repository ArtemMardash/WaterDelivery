using Mediator;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.CustomersAddresses.Dtos;
using WaterDelivery.Backend.Features.Shared;

namespace WaterDelivery.Backend.Features.CustomersAddresses;

public class CreateCustomerAddressesUseCase: IRequestHandler<CreateCustomerAddressesDto, Guid>
{
    private readonly ICustomerAddressesRepository _addressesRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCustomerAddressesUseCase(ICustomerAddressesRepository addressesRepository, IUnitOfWork unitOfWork)
    {
        _addressesRepository = addressesRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async ValueTask<Guid> Handle(CreateCustomerAddressesDto request, CancellationToken cancellationToken)
    {
        var customerAddresses =
            new CustomerAddresses(request.CustomerId,
                request.Addresses.Select(a => a.ToEntity()).ToList());
        var result = await _addressesRepository.CreateCustomerAddressesAsync(customerAddresses, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return customerAddresses.Id;
    }
}
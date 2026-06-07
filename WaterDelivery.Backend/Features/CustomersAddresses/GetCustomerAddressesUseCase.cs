using Mediator;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Contracts.CustomersAddresses.Dtos;

namespace WaterDelivery.Backend.Features.CustomersAddresses;

public class GetCustomerAddressesUseCase: IRequestHandler<GetCustomerAddressesDto, GetCustomerAddressesResultDto>
{
    private readonly ICustomerAddressesRepository _customerAddressesRepository;

    public GetCustomerAddressesUseCase(ICustomerAddressesRepository customerAddressesRepository)
    {
        _customerAddressesRepository = customerAddressesRepository;
    }
    
    public async ValueTask<GetCustomerAddressesResultDto> Handle(GetCustomerAddressesDto request, CancellationToken cancellationToken)
    {
        var records = await _customerAddressesRepository.GetAllCustomerAddresses(request.CustomerId, cancellationToken);

        // A customer may have no address book yet (first-time checkout) -> return an empty list rather than throwing.
        var addresses = records
            .SelectMany(r => r.Addresses)
            .Where(a => !a.IsDeleted)
            .Select(a => a.ToDto())
            .ToList();

        return new GetCustomerAddressesResultDto
        {
            CustomerId = request.CustomerId,
            Addresses = addresses
        };
    }
}
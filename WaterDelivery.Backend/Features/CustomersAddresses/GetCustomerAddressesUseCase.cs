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
        var result = await _customerAddressesRepository.GetCustomerAddressesByIdAsync(request.CustomerId, cancellationToken);
        return new GetCustomerAddressesResultDto
        {
            CustomerId = result.CustomerId,
            Addresses = result.Addresses.Select(s=>s.ToDto()).ToList()
        };
    }
}
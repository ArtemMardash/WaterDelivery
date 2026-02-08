using Mediator;
using WaterDelivery.Backend.Features.CustomersAddresses.Dtos;
using WaterDelivery.Backend.Features.Shared;

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
        var result = await _customerAddressesRepository.GetCustomerAddressesByIdAsync(request.Id, cancellationToken);
        return new GetCustomerAddressesResultDto
        {
            Id = result.Id,
            CustomerId = result.CustomerId,
            Addresses = result.Addresses.Select(s=>s.ToDto()).ToList()
        };
    }
}
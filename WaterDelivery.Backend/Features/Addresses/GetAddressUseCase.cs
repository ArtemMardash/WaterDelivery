using Mediator;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Contracts.Addresses.Dtos;

namespace WaterDelivery.Backend.Features.Addresses;

public class GetAddressUseCase: IRequestHandler<GetAddressDto, GetAddressResultDto>
{
    private readonly IAddressRepository _addressRepository;

    public GetAddressUseCase(IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }
    
    public async ValueTask<GetAddressResultDto> Handle(GetAddressDto request, CancellationToken cancellationToken)
    {
        var result = await _addressRepository.GetAddressAsync(request.Id, cancellationToken);
        return new GetAddressResultDto
        {
            Id = result.Id,
            Street = result.Street,
            HouseNumber = result.HouseNumber,
            AptNumber = result.AptNumber,
            City = result.City,
            State = result.State
        };
    }
}
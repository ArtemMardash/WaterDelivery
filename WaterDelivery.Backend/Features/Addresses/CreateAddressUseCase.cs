using Mediator;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.Addresses.Dtos;
using WaterDelivery.Backend.Features.Shared;

namespace WaterDelivery.Backend.Features.Addresses;

public class CreateAddressUseCase: IRequestHandler<CreateAddressDto, Guid>
{
    private readonly IAddressRepository _addressRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAddressUseCase(IAddressRepository addressRepository, IUnitOfWork unitOfWork)
    {
        _addressRepository = addressRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async ValueTask<Guid> Handle(CreateAddressDto request, CancellationToken cancellationToken)
    {
        var address = new Address(request.Street, request.HouseNumber, request.AptNumber, request.City, request.State);

        await _addressRepository.CreateAddressAsync(address, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return address.Id;
    }
}
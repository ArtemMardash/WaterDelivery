using Mediator;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.Addresses.Dtos;
using WaterDelivery.Backend.Features.Shared;

namespace WaterDelivery.Backend.Features.Addresses;

public class UpdateAddressUseCase: IRequestHandler<UpdateAddressDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAddressRepository _addressRepository;

    public UpdateAddressUseCase(IUnitOfWork unitOfWork, IAddressRepository addressRepository)
    {
        _unitOfWork = unitOfWork;
        _addressRepository = addressRepository;
    }
    
    public async ValueTask<Unit> Handle(UpdateAddressDto request, CancellationToken cancellationToken)
    {
        var addressToUpdate = new Address(
            request.Id, 
            request.Street, 
            request.HouseNumber, 
            request.AptNumber,
            request.City, 
            request.State,
            request.isDeleted);

        await _addressRepository.UpdateAddressAsync(addressToUpdate, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
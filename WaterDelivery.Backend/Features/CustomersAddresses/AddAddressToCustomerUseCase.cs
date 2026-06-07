using Mediator;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Contracts.CustomersAddresses.Dtos;

namespace WaterDelivery.Backend.Features.CustomersAddresses;

/// <summary>
/// Adds one address to the customer's address book.
/// - If the customer has no record yet, a new CustomerAddresses is created.
/// - If an identical (non-deleted) address already exists, its id is returned and nothing is duplicated.
/// </summary>
public class AddAddressToCustomerUseCase : IRequestHandler<AddAddressToCustomerDto, Guid>
{
    private readonly ICustomerAddressesRepository _customerAddressesRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddAddressToCustomerUseCase(ICustomerAddressesRepository customerAddressesRepository, IUnitOfWork unitOfWork)
    {
        _customerAddressesRepository = customerAddressesRepository;
        _unitOfWork = unitOfWork;
    }

    public async ValueTask<Guid> Handle(AddAddressToCustomerDto request, CancellationToken cancellationToken)
    {
        // A brand-new address arrives with an empty Id; build it with a generated Id (5-arg ctor)
        // so it can be referenced later. An existing address keeps its Id (7-arg ctor via ToEntity).
        var newAddress = request.Address.Id == Guid.Empty
            ? new Address(
                request.Address.Street,
                request.Address.HouseNumber,
                request.Address.AptNumber,
                request.Address.City,
                request.Address.State)
            : request.Address.ToEntity();

        var records = await _customerAddressesRepository.GetAllCustomerAddresses(request.CustomerId, cancellationToken);
        var record = records.FirstOrDefault();

        if (record is null)
        {
            // First address for this customer -> create the book.
            record = new CustomerAddresses(request.CustomerId, new List<Address> { newAddress });
            await _customerAddressesRepository.CreateCustomerAddressesAsync(record, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return newAddress.Id;
        }

        // Reuse an identical address if the customer already has it.
        var existing = record.Addresses.FirstOrDefault(a => !a.IsDeleted && IsSameAddress(a, newAddress));
        if (existing is not null)
        {
            return existing.Id;
        }

        record.Addresses.Add(newAddress);
        await _customerAddressesRepository.UpdateCustomerAddressesAsync(record, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return newAddress.Id;
    }

    private static bool IsSameAddress(Address a, Address b)
    {
        return string.Equals(a.Street, b.Street, StringComparison.OrdinalIgnoreCase)
               && string.Equals(a.HouseNumber, b.HouseNumber, StringComparison.OrdinalIgnoreCase)
               && string.Equals(a.AptNumber ?? string.Empty, b.AptNumber ?? string.Empty, StringComparison.OrdinalIgnoreCase)
               && string.Equals(a.City, b.City, StringComparison.OrdinalIgnoreCase)
               && string.Equals(a.State, b.State, StringComparison.OrdinalIgnoreCase);
    }
}

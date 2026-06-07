using FluentAssertions;
using NSubstitute;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.CustomersAddresses;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Contracts.Addresses.Dtos;
using WaterDelivery.Contracts.CustomersAddresses.Dtos;

namespace WaterDelivery.Tests.UseCaseTests;

public class CustomersAddressesUseCaseTests
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ICustomerAddressesRepository _customerAddressesRepository;
    
    public CustomersAddressesUseCaseTests()
    {
        _customerAddressesRepository = Substitute.For<ICustomerAddressesRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _unitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
    }

    [Fact]
    public async Task Create_Customer_Addresses_Should_Success()
    {
        var dto = new CreateCustomerAddressesDto
        {
            CustomerId = Guid.NewGuid(),
            Addresses = new List<AddressDto>()
        };

        var useCase = new CreateCustomerAddressesUseCase(_customerAddressesRepository, _unitOfWork);
        var result = await useCase.Handle(dto, CancellationToken.None);

        result.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Get_Customer_Addresses_Should_Success()
    {
        var customerId = Guid.NewGuid();
        var addresses = new CustomerAddresses(customerId, new List<Address>());
        _customerAddressesRepository.GetAllCustomerAddresses(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(new List<CustomerAddresses> { addresses });
        var useCase = new GetCustomerAddressesUseCase(_customerAddressesRepository);
        var result = await useCase.Handle(new GetCustomerAddressesDto
        {
            CustomerId = customerId
        }, CancellationToken.None);

        result.CustomerId.Should().Be(customerId);
    }

    [Fact]
    public async Task Add_Address_To_New_Customer_Should_Create_Book()
    {
        var customerId = Guid.NewGuid();
        _customerAddressesRepository.GetAllCustomerAddresses(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(new List<CustomerAddresses>());

        var dto = new AddAddressToCustomerDto
        {
            CustomerId = customerId,
            Address = new AddressDto
            {
                Id = Guid.Empty,
                Street = "Main Street",
                HouseNumber = "42",
                AptNumber = null,
                City = "New York",
                State = "NY"
            }
        };

        var useCase = new AddAddressToCustomerUseCase(_customerAddressesRepository, _unitOfWork);
        var result = await useCase.Handle(dto, CancellationToken.None);

        result.Should().NotBeEmpty();
        await _customerAddressesRepository.Received(1)
            .CreateCustomerAddressesAsync(Arg.Any<CustomerAddresses>(), Arg.Any<CancellationToken>());
    }
}
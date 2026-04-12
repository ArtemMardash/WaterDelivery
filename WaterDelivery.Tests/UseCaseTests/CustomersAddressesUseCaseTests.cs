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
        var addresses = new CustomerAddresses(Guid.NewGuid(), new List<Address>());
        _customerAddressesRepository.GetCustomerAddressesByIdAsync(Arg.Any<Guid>(), CancellationToken.None)
            .Returns(addresses);
        var useCase = new GetCustomerAddressesUseCase(_customerAddressesRepository);
        var result = await useCase.Handle(new GetCustomerAddressesDto
        {
            CustomerId = Guid.NewGuid()
        }, CancellationToken.None);

        result.CustomerId.Should().Be(addresses.CustomerId);
    }
}
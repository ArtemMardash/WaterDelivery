using FluentAssertions;
using NSubstitute;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.Addresses;
using WaterDelivery.Backend.Features.Addresses.Dtos;
using WaterDelivery.Backend.Features.Shared;
using Address = WaterDelivery.Backend.Core.Entities.Address;

namespace WaterDelivery.Tests.UseCaseTests;

public class AddressUseCaseTests
{
    private readonly IAddressRepository _addressRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public AddressUseCaseTests()
    {
        _addressRepository = Substitute.For<IAddressRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _unitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
    }

    [Fact]
    public async Task Create_Address_Should_Success()
    {
        var dto = new CreateAddressDto
        {
            Street = "street",
            HouseNumber = "2",
            AptNumber = "3",
            City = "Nyc",
            State = "NY"
        };

        var createAddressUseCase = new CreateAddressUseCase(_addressRepository, _unitOfWork);
        var result = await createAddressUseCase.Handle(dto, CancellationToken.None);

        result.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Get_Address_Should_Success()
    {
        var address = new Address("street", "2E", "3B", "NYC", "NY");
        _addressRepository.GetAddressAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(address);

        var getAddressUseCase = new GetAddressUseCase(_addressRepository);
        var result = await getAddressUseCase.Handle(new GetAddressDto
        {
            Id = Guid.NewGuid()
        }, CancellationToken.None);

        result.Id.Should().Be(address.Id);
        result.HouseNumber.Should().Be(address.HouseNumber);
        result.AptNumber.Should().Be(address.AptNumber);
        result.City.Should().Be(address.City);
        result.State.Should().Be(address.State);
        result.Street.Should().Be(address.Street);
    }
}
using FluentAssertions;
using Mediator;
using MongoDB.Driver;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.Addresses.Dtos;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Backend.Infrastructure.Persistence;

namespace WaterDelivery.Tests.IntegrationTests;

public class AddressUseCases: IDisposable
{
    private readonly IAddressRepository _addressRepository;
    private readonly WaterDeliveryContext _waterDeliveryContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    private readonly IntegrationTestHelper _dbService = new IntegrationTestHelper();
    private static Guid _addressId;

    public AddressUseCases()
    {
        _addressRepository = _dbService.GetRequiredService<IAddressRepository>();
        _mediator = _dbService.GetRequiredService<IMediator>();
        _unitOfWork = _dbService.GetRequiredService<IUnitOfWork>();
        _waterDeliveryContext = _dbService.GetRequiredService<WaterDeliveryContext>();
        _addressId = _dbService.CreateAddress(_addressRepository, _unitOfWork);
    }

    [Fact]
    public async Task Create_Address_Use_Case_Should_Success()
    {
        var request = new CreateAddressDto
        {
            Street = "Kings Hwy",
            HouseNumber = "10",
            AptNumber = "5B",
            City = "Brooklyn",
            State = "NY"
        };

        var result = await _mediator.Send(request, CancellationToken.None);

        result.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public async Task Update_Address_Use_Case_Should_Success()
    {
        var createdId = await _mediator.Send(new CreateAddressDto
        {
            Street = "Ocean Pkwy",
            HouseNumber = "11",
            AptNumber = "2A",
            City = "Brooklyn",
            State = "NY"
        }, CancellationToken.None);

        var update = new UpdateAddressDto
        {
            Id = createdId,
            Street = "Ocean Pkwy UPDATED",
            HouseNumber = "111",
            AptNumber = "9C",
            City = "Brooklyn",
            State = "NY",
            isDeleted = false
        };

        await _mediator.Send(update, CancellationToken.None);

        var address = await _waterDeliveryContext
            .GetCollection<Address>("address")
            .Find(a => a.Id == createdId)
            .FirstOrDefaultAsync();

        address.Should().NotBeNull();
        address.Street.Should().Be("Ocean Pkwy UPDATED");
        address.HouseNumber.Should().Be("111");
        address.AptNumber.Should().Be("9C");
        address.City.Should().Be("Brooklyn");
        address.State.Should().Be("NY");
    }

    [Fact]
    public async Task Delete_Address_Use_Case_Should_Success()
    {
        await _mediator.Send(new DeleteAddressDto { Id = _addressId }, CancellationToken.None);

        var exists = await _waterDeliveryContext
            .GetCollection<Address>("address") 
            .Find(a => a.Id == _addressId && a.IsDeleted)
            .AnyAsync();

        exists.Should().BeTrue();
    }

    [Fact]
    public async Task Get_Address_Use_Case_Should_Success()
    {
        var result = await _mediator.Send(new GetAddressDto { Id = _addressId }, CancellationToken.None);

        result.Id.Should().Be(_addressId);
        result.Street.Should().Be("Kings Hwy");
        result.HouseNumber.Should().Be("10");
        result.AptNumber.Should().Be("5B");
        result.City.Should().Be("Brooklyn");
        result.State.Should().Be("NY");
    }
    
    public void Dispose()
    {
        _dbService.DeleteDb(_waterDeliveryContext);
        _unitOfWork.Dispose();
    }
}
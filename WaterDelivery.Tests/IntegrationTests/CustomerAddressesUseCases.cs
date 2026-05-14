using FluentAssertions;
using Mediator;
using MongoDB.Driver;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Backend.Infrastructure.Persistence;
using WaterDelivery.Contracts.Addresses.Dtos;
using WaterDelivery.Contracts.CustomersAddresses.Dtos;

namespace WaterDelivery.Tests.IntegrationTests;

public class CustomerAddressesUseCases: IAsyncLifetime
{
    private readonly ICustomerAddressesRepository _customerAddressesRepository;
    private readonly WaterDeliveryContext _waterDeliveryContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    private readonly IntegrationTestHelper _dbService = new IntegrationTestHelper();
    private static Guid _customerAddressesId;

    public CustomerAddressesUseCases()
    {
        _customerAddressesRepository = _dbService.GetRequiredService<ICustomerAddressesRepository>();
        _mediator = _dbService.GetRequiredService<IMediator>();
        _unitOfWork = _dbService.GetRequiredService<IUnitOfWork>();
        _waterDeliveryContext = _dbService.GetRequiredService<WaterDeliveryContext>();
        _customerAddressesId = _dbService.CreateCustomerAddresses(_customerAddressesRepository, _unitOfWork);
    }

    [Fact]
    public async Task Create_CustomerAddresses_Use_Case_Should_Success()
    {
        var request = new CreateCustomerAddressesDto
        {
            CustomerId = Guid.NewGuid(),
            Addresses = new List<AddressDto>()
        };

        var result = await _mediator.Send(request, CancellationToken.None);

        result.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public async Task Update_CustomerAddresses_Use_Case_Should_Success()
    {
        var createdId = await _mediator.Send(new CreateCustomerAddressesDto
        {
            CustomerId = Guid.NewGuid(),
            Addresses = new List<AddressDto>()
        }, CancellationToken.None);

        var updatedCustomerId = Guid.NewGuid();

        var update = new UpdateCustomerAddressesDto
        {
            Id = createdId,
            CustomerId = updatedCustomerId,
            Addresses = new List<AddressDto>()
        };

        await _mediator.Send(update, CancellationToken.None);

        var customerAddresses = await _waterDeliveryContext
            .GetCollection<CustomerAddresses>("customerAddresses")
            .Find(ca => ca.Id == createdId)
            .FirstOrDefaultAsync();

        customerAddresses.Should().NotBeNull();
        customerAddresses.CustomerId.Should().Be(updatedCustomerId);
        customerAddresses.Addresses.Should().NotBeNull();
        customerAddresses.Addresses.Should().HaveCount(0);
    }

    [Fact]
    public async Task Delete_CustomerAddresses_Use_Case_Should_Success()
    {
        await _mediator.Send(new DeleteCustomerAddressesDto { Id = _customerAddressesId }, CancellationToken.None);

        var customerAddresses = await _waterDeliveryContext
            .GetCollection<CustomerAddresses>("customerAddresses")
            .Find(ca => ca.Id == _customerAddressesId)
            .FirstOrDefaultAsync();

        customerAddresses.Should().BeNull();
    }

    [Fact]
    public async Task Get_CustomerAddresses_Use_Case_Should_Success()
    {
        var result = await _mediator.Send(new GetCustomerAddressesDto { CustomerId = _customerAddressesId }, CancellationToken.None);

        result.CustomerId.Should().NotBe(Guid.Empty);
        result.Addresses.Should().NotBeNull();
        result.Addresses.Should().HaveCount(0);
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        _dbService.DeleteDb(_waterDeliveryContext);
        _unitOfWork.Dispose();
    }
}
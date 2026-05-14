using FluentAssertions;
using Mediator;
using MongoDB.Driver;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Backend.Infrastructure.Persistence;
using WaterDelivery.Contracts.Addresses.Dtos;
using WaterDelivery.Contracts.Deliveries.Dtos;
using WaterDelivery.Contracts.Enums;
using WaterDelivery.Contracts.Orders.Dtos;

namespace WaterDelivery.Tests.IntegrationTests;

public class DeliveryUseCases: IAsyncLifetime
{
    private readonly IDeliveryRepository _deliveryRepository;
    private readonly WaterDeliveryContext _waterDeliveryContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    private readonly IntegrationTestHelper _dbService = new IntegrationTestHelper();
    private static Guid _deliveryId;

    public DeliveryUseCases()
    {
        _deliveryRepository = _dbService.GetRequiredService<IDeliveryRepository>();
        _mediator = _dbService.GetRequiredService<IMediator>();
        _unitOfWork = _dbService.GetRequiredService<IUnitOfWork>();
        _waterDeliveryContext = _dbService.GetRequiredService<WaterDeliveryContext>();
        _deliveryId = _dbService.CreateDelivery(_deliveryRepository, _unitOfWork);
    }

    [Fact]
    public async Task Create_Delivery_Use_Case_Should_Success()
    {
        var request = new CreateDeliveryDto
        {
            DeliveryManId = Guid.NewGuid(),
            Order = new OrderDto
            {
                Id = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                Items = new List<OrderItemDto>()
            },
            Address = new AddressDto
            {
                Id = Guid.NewGuid(),
                Street = "Street",
                HouseNumber = "1509",
                AptNumber = "2b",
                City = "Brooklyn",
                State = "NY",
            },
            Status = DeliveryStatus.Assembly
        };

        var result = await _mediator.Send(request, CancellationToken.None);

        result.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public async Task Update_Delivery_Use_Case_Should_Success()
    {
        var createdId = await _mediator.Send(new CreateDeliveryDto
        {
            DeliveryManId = Guid.NewGuid(),
            Order = new OrderDto
            {
                Id = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                Items = new List<OrderItemDto>()
            },
            Address = new AddressDto
            {
                Id = Guid.NewGuid(),
                Street = "Street",
                HouseNumber = "1509",
                AptNumber = "2b",
                City = "Brooklyn",
                State = "NY",
            },
            Status = DeliveryStatus.Assembly
        }, CancellationToken.None);

        var updatedDeliveryManId = Guid.NewGuid();

        var update = new UpdateDeliveryDto
        {
            Id = createdId,
            DeliveryManId = updatedDeliveryManId,
            Order = new OrderDto
            {
                Id = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                Items = new List<OrderItemDto>(),
            },
            Address = new AddressDto
            {
                Id = Guid.NewGuid(),
                Street = "Ocean Pkwy",
                HouseNumber = "111",
                AptNumber = "9C",
                City = "Brooklyn",
                State = "NY",
            },
            Status = DeliveryStatus.Delivered
        };

        await _mediator.Send(update, CancellationToken.None);

        var delivery = await _waterDeliveryContext
            .GetCollection<Delivery>("delivery")
            .Find(d => d.Id == createdId)
            .FirstOrDefaultAsync();

        delivery.Should().NotBeNull();
        delivery.DeliveryManId.Should().Be(updatedDeliveryManId);
        delivery.Order.Should().NotBeNull();
        delivery.Order.CustomerId.Should().Be(update.Order.CustomerId);
        delivery.Address.Should().NotBeNull();
        delivery.Address.Street.Should().Be("Ocean Pkwy");
        delivery.Address.HouseNumber.Should().Be("111");
        delivery.Address.AptNumber.Should().Be("9C");
        delivery.Address.City.Should().Be("Brooklyn");
        delivery.Address.State.Should().Be("NY");
        delivery.Status.Should().Be(DeliveryStatus.Delivered);
    }

    [Fact]
    public async Task Delete_Delivery_Use_Case_Should_Success()
    {
        await _mediator.Send(new DeleteDeliveryDto { Id = _deliveryId }, CancellationToken.None);

        var exists = await _waterDeliveryContext
            .GetCollection<Delivery>("delivery")
            .Find(d => d.Id == _deliveryId)
            .AnyAsync();

        exists.Should().BeFalse();
    }

    [Fact]
    public async Task Get_Delivery_Use_Case_Should_Success()
    {
        var result = await _mediator.Send(new GetDeliveryDto { Id = _deliveryId }, CancellationToken.None);

        result.Id.Should().Be(_deliveryId);
        result.DeliveryManId.Should().NotBe(Guid.Empty);
        result.Order.Should().NotBeNull();
        result.Address.Should().NotBeNull();
        result.Address.Street.Should().Be("street");
        result.Address.HouseNumber.Should().Be("1509");
        result.Address.AptNumber.Should().Be("2b");
        result.Address.City.Should().Be("Brooklyn");
        result.Address.State.Should().Be("NY");
        result.Status.Should().Be(DeliveryStatus.Assembly);
    }
    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        _dbService.DeleteDb(_waterDeliveryContext);
        _unitOfWork.Dispose();
    }
}
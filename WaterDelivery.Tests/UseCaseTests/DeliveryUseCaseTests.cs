using FluentAssertions;
using NSubstitute;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Core.Enums;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.Addresses.Dtos;
using WaterDelivery.Backend.Features.Deliveries;
using WaterDelivery.Backend.Features.Deliveries.Dtos;
using WaterDelivery.Backend.Features.Orders.Dtos;
using WaterDelivery.Backend.Features.Shared;

namespace WaterDelivery.Tests.UseCaseTests;

public class DeliveryUseCaseTests
{
    private readonly IDeliveryRepository _deliveryRepository;

    private readonly IUnitOfWork _unitOfWork;
    
    public DeliveryUseCaseTests()
    {
        _deliveryRepository = Substitute.For<IDeliveryRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _unitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
    }

    [Fact]
    public async Task Create_Delivery_Should_Success()
    {
        var dto = new CreateDeliveryDto
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
                Street = "street",
                HouseNumber = "house",
                AptNumber = "a2",
                City = "city",
                State = "State",
                isDeleted = false
            },
            Status = DeliveryStatus.Assembly
        };

        var useCase = new CreateDeliveryUseCase(_unitOfWork, _deliveryRepository);

        var result =await useCase.Handle(dto, CancellationToken.None);

        result.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Get_Delivery_Should_Success()
    {
        var delivery = new Delivery(
            Guid.NewGuid(), 
            new Order(Guid.NewGuid(), Guid.NewGuid(), new List<OrderItem>()),
            new Address("street", "house", "2a", "city", "state"), 
            DeliveryStatus.Assembly);

        _deliveryRepository.GetDeliveryByIdAsync(Arg.Any<Guid>(), CancellationToken.None).Returns(delivery);

        var useCase = new GetDeliveryUseCase(_deliveryRepository);
        var result = await useCase.Handle(new GetDeliveryDto
        {
            Id = Guid.NewGuid()
        }, CancellationToken.None);

        result.Id.Should().Be(delivery.Id);
        result.Address.City.Should().Be(delivery.Address.City);
        result.Address.Id.Should().Be(delivery.Address.Id);
        result.Address.AptNumber.Should().Be(delivery.Address.AptNumber);
        result.Address.HouseNumber.Should().Be(delivery.Address.HouseNumber);
        result.Address.State.Should().Be(delivery.Address.State);
        result.Address.Street.Should().Be(delivery.Address.Street);
        result.Address.isDeleted.Should().Be(delivery.Address.IsDeleted);
        result.DeliveryManId.Should().Be(delivery.DeliveryManId);
        result.Status.Should().Be(delivery.Status);
        result.Order.CustomerId.Should().Be(delivery.Order.CustomerId);
        result.Order.Id.Should().Be(delivery.Order.Id);
        result.Order.Items.Count.Should().Be(delivery.Order.Items.Count);
    }
}
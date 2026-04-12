using FluentAssertions;
using Mediator;
using MongoDB.Driver;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Backend.Infrastructure.Persistence;
using WaterDelivery.Contracts.Orders.Dtos;

namespace WaterDelivery.Tests.IntegrationTests;

public class OrderUseCases: IDisposable
{
    private readonly IOrderRepository _orderRepository;
    private readonly WaterDeliveryContext _waterDeliveryContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    private readonly IntegrationTestHelper _dbService = new IntegrationTestHelper();
    private static Guid _orderId;

    public OrderUseCases()
    {
        _orderRepository = _dbService.GetRequiredService<IOrderRepository>();
        _mediator = _dbService.GetRequiredService<IMediator>();
        _unitOfWork = _dbService.GetRequiredService<IUnitOfWork>();
        _waterDeliveryContext = _dbService.GetRequiredService<WaterDeliveryContext>();
        _orderId = _dbService.CreateOrder(_orderRepository, _unitOfWork);
    }

    [Fact]
    public async Task Create_Order_Use_Case_Should_Success()
    {
        var request = new CreateOrderDto
        {
            CustomerId = Guid.NewGuid(),
            Items = new List<OrderItemDto>()
        };

        var result = await _mediator.Send(request, CancellationToken.None);

        result.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public async Task Update_Order_Use_Case_Should_Success()
    {
        var createdId = await _mediator.Send(new CreateOrderDto
        {
            CustomerId = Guid.NewGuid(),
            Items = new List<OrderItemDto>()
        }, CancellationToken.None);

        var updatedCustomerId = Guid.NewGuid();

        var update = new UpdateOrderDto
        {
            Id = createdId,
            CustomerId = updatedCustomerId,
            Items = new List<OrderItemDto>()
        };

        await _mediator.Send(update, CancellationToken.None);

        var order = await _waterDeliveryContext
            .GetCollection<Order>("order")
            .Find(o => o.Id == createdId)
            .FirstOrDefaultAsync();

        order.Should().NotBeNull();
        order.CustomerId.Should().Be(updatedCustomerId);
        order.Items.Should().NotBeNull();
        order.Items.Should().HaveCount(0);
    }

    [Fact]
    public async Task Delete_Order_Use_Case_Should_Success()
    {
        await _mediator.Send(new DeleteOrderDto { Id = _orderId }, CancellationToken.None);

        var order = await _waterDeliveryContext
            .GetCollection<Order>("order")
            .Find(o => o.Id == _orderId)
            .FirstOrDefaultAsync();

        order.Should().BeNull();
    }

    [Fact]
    public async Task Get_Order_Use_Case_Should_Success()
    {
        var result = await _mediator.Send(new GetOrderDto { Id = _orderId }, CancellationToken.None);

        result.Id.Should().Be(_orderId);
        result.CustomerId.Should().NotBe(Guid.Empty);
        result.Items.Should().NotBeNull();
        result.Items.Should().HaveCount(0);
    }

    public void Dispose()
    {
        _dbService.DeleteDb(_waterDeliveryContext);
        _unitOfWork.Dispose();
    }
}
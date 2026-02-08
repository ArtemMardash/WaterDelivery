using FluentAssertions;
using MongoDB.Driver;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Backend.Infrastructure.Persistence;
using WaterDelivery.Backend.Infrastructure.Persistence.DbEntities;
using WaterDelivery.Backend.Infrastructure.Persistence.Repositories;

namespace WaterDelivery.Tests.RepositoryTests;

//TODO fail methods for all exceptions in repository
public class OrderRepositoryTests: BaseTest
{
    private readonly WaterDeliveryContext _context;
    private readonly IOrderRepository _orderRepository;
    private readonly IMongoCollection<OrderDb> _order;

    private readonly string _dbName = $"orderTest_{Guid.NewGuid()}";

    public OrderRepositoryTests()
    {
        _context = new WaterDeliveryContext("mongodb://root:password@localhost:27017", _dbName);
        _order = _context.GetCollection<OrderDb>("order");
        _orderRepository = new OrderRepository(_context);
    }
    

    [Fact]
    public async Task Create_Order_Async_Should_Success()
    {
        var order = new Order(Guid.NewGuid(), new List<OrderItem>());
        var resultId =
            await _orderRepository.CreateOrderAsync(order, CancellationToken.None);

        resultId.Should().Be(order.Id);
    }

    [Fact]
    public async Task Update_Order_Async_Should_Success()
    {
        var order = new Order(Guid.NewGuid(), new List<OrderItem>());
        var resultId =
            await _orderRepository.CreateOrderAsync(order, CancellationToken.None);


        var orderNew = new Order(resultId,Guid.NewGuid(), new List<OrderItem>());

        await _orderRepository.UpdateOrderAsync(orderNew, CancellationToken.None);
        var orderDb = await _order.Find(u => u.Id == resultId).FirstOrDefaultAsync(CancellationToken.None);

        orderDb.CustomerId.Should().Be(orderNew.CustomerId);
        orderDb.Id.Should().Be(orderNew.Id);
    }

    [Fact]
    public async Task Get_Order_By_Id_Async_ShouldSuccess()
    {
        var order = new Order(Guid.NewGuid(), new List<OrderItem>());
        var resultId =
            await _orderRepository.CreateOrderAsync(order, CancellationToken.None);

        var orderById = await _orderRepository.GetOrderByIdAsync(resultId, CancellationToken.None);

        orderById.CustomerId.Should().Be(order.CustomerId);
        orderById.Id.Should().Be(order.Id);
    }

    [Fact]
    public async Task Get_All_Customer_Orders_ShouldSuccess()
    {
        var custId = Guid.NewGuid();
        var order = new Order(custId, new List<OrderItem>());
        var resultId =
            await _orderRepository.CreateOrderAsync(order, CancellationToken.None);
        
        var custOrders = await _orderRepository.GetAllCustomerOrdersAsync(custId, CancellationToken.None);

        custOrders.Count.Should().Be(1);
    }
    
    [Fact]
    public async Task Delete_Order_Should_Success()
    {
        var order = new Order(Guid.NewGuid(), new List<OrderItem>());
        var resultId =
            await _orderRepository.CreateOrderAsync(order, CancellationToken.None);

        await _orderRepository.DeleteOrderAsync(resultId, CancellationToken.None);

        var test = async ()=>await  _orderRepository.GetOrderByIdAsync(resultId, CancellationToken.None);
        await test.Should().ThrowAsync<InvalidOperationException>();
    }
    
    public override void Dispose()
    {
        _context.DeleteDb(_dbName);
    }
}
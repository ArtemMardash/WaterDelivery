using FluentAssertions;
using MongoDB.Driver;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Core.Enums;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Backend.Infrastructure.Persistence;
using WaterDelivery.Backend.Infrastructure.Persistence.DbEntities;
using WaterDelivery.Backend.Infrastructure.Persistence.Repositories;

namespace WaterDelivery.Tests.RepositoryTests;

//TODO fail methods for all exceptions in repository
public class DeliveryRepositoryTests: BaseTest
{
    private readonly WaterDeliveryContext _context;
    private readonly IDeliveryRepository _deliveryRepository;
    private readonly IMongoCollection<DeliveryDb> _delivery;

    private readonly string _dbName = $"addressTest_{Guid.NewGuid()}";

    public DeliveryRepositoryTests()
    {
        _context = new WaterDeliveryContext("mongodb://root:password@localhost:27017", _dbName);
        _delivery = _context.GetCollection<DeliveryDb>("deliveries");
        _deliveryRepository = new DeliveryRepository(_context);
    }

    [Fact]
    public async Task Create_Delivery_Async_Should_Success()
    {
        var address = new Address("street", "hr12", "2b", "City", "New York");
        var delivery = new Delivery(Guid.NewGuid(), new Order(Guid.NewGuid(), new List<OrderItem>()), address,
            DeliveryStatus.Assembly);
        var resultId = await _deliveryRepository.CreateDeliveryAsync(delivery, CancellationToken.None);

        resultId.Should().Be(delivery.Id);
    }

    [Fact]
    public async Task Update_Delivery_Async_Should_Success()
    {
        var address = new Address("street", "hr12", "2b", "City", "New York");
        var delivery = new Delivery(Guid.NewGuid(), new Order(Guid.NewGuid(), new List<OrderItem>()), address,
            DeliveryStatus.Assembly);
        var resultId = await _deliveryRepository.CreateDeliveryAsync(delivery, CancellationToken.None);


        var deliveryNew = new Delivery(resultId, Guid.NewGuid(), new Order(Guid.NewGuid(), new List<OrderItem>()),
            address, DeliveryStatus.TransferredDeliveryService);

        await _deliveryRepository.UpdateDeliveryAsync(deliveryNew, CancellationToken.None);
        var deliveryByIdAsync = await _deliveryRepository.GetDeliveryByIdAsync(resultId, CancellationToken.None);

        deliveryByIdAsync.Address.AptNumber.Should().Be(deliveryNew.Address.AptNumber);
        deliveryByIdAsync.Address.Street.Should().Be(deliveryNew.Address.Street);
        deliveryByIdAsync.Address.State.Should().Be(deliveryNew.Address.State);
        deliveryByIdAsync.Address.HouseNumber.Should().Be(deliveryNew.Address.HouseNumber);
        deliveryByIdAsync.Address.City.Should().Be(deliveryNew.Address.City);
        deliveryByIdAsync.Address.Id.Should().Be(deliveryNew.Address.Id);
        deliveryByIdAsync.DeliveryManId.Should().Be(deliveryNew.DeliveryManId);
        deliveryByIdAsync.Status.Should().Be(deliveryNew.Status);
        deliveryByIdAsync.Id.Should().Be(deliveryNew.Id);
    }

    [Fact]
    public async Task Get_Delivery_By_Id_Async_ShouldSuccess()
    {
        var address = new Address("street", "hr12", "2b", "City", "New York");
        var delivery = new Delivery(Guid.NewGuid(), new Order(Guid.NewGuid(), new List<OrderItem>()), address,
            DeliveryStatus.Assembly);
        var resultId = await _deliveryRepository.CreateDeliveryAsync(delivery, CancellationToken.None);


        var deliveryById = await _deliveryRepository.GetDeliveryByIdAsync(resultId, CancellationToken.None);

        deliveryById.Address.City.Should().Be(delivery.Address.City);
        deliveryById.Address.AptNumber.Should().Be(delivery.Address.AptNumber);
        deliveryById.Address.HouseNumber.Should().Be(delivery.Address.HouseNumber);
        deliveryById.Address.Id.Should().Be(delivery.Address.Id);
        deliveryById.Address.Street.Should().Be(delivery.Address.Street);
        deliveryById.DeliveryManId.Should().Be(delivery.DeliveryManId);
        deliveryById.Status.Should().Be(delivery.Status);
        deliveryById.Id.Should().Be(delivery.Id);
    }
    
    [Fact]
    public async Task Delete_Delivery_Should_Success()
    {
        var address = new Address("street", "hr12", "2b", "City", "New York");
        var delivery = new Delivery(Guid.NewGuid(), new Order(Guid.NewGuid(), new List<OrderItem>()), address,
            DeliveryStatus.Assembly);
        var resultId = await _deliveryRepository.CreateDeliveryAsync(delivery, CancellationToken.None);

        (await _delivery.Find(u => u.Id == resultId).FirstOrDefaultAsync(CancellationToken.None)).Should().BeNull();
    }
    
    public override void Dispose()
    {
        _context.DeleteDb(_dbName);
    }
}
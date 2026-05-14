using FluentAssertions;
using MongoDB.Driver;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Backend.Infrastructure.Persistence;
using WaterDelivery.Backend.Infrastructure.Persistence.DbEntities;
using WaterDelivery.Backend.Infrastructure.Persistence.Repositories;

namespace WaterDelivery.Tests.RepositoryTests;

//TODO fail methods for all exceptions in repository
public class CartRepositoryTests : BaseTest
{
    private readonly WaterDeliveryContext _context;
    private readonly ICartRepository _cartRepository;
    private readonly IMongoCollection<CartDb> _cart;

    private readonly string _dbName = $"cartTest_{Guid.NewGuid()}";

    public CartRepositoryTests()
    {
        _context = new WaterDeliveryContext("mongodb://root:password@localhost:27017/?authSource=admin", _dbName);
        _cart = _context.GetCollection<CartDb>("cart");
        _cartRepository = new CartRepository(_context);
    }

    [Fact]
    public async Task Create_Cart_Async_Should_Success()
    {
        var cart = new Cart(Guid.NewGuid(), new Dictionary<Guid, int>());

        var resultId = await _cartRepository.CreateCartAsync(cart, CancellationToken.None);

        resultId.Should().Be(cart.CustomerId);
    }

    [Fact]
    public async Task Update_Cart_Async_Should_Success()
    {
        var customerId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        var cart = new Cart(customerId, new Dictionary<Guid, int>());
        await _cartRepository.CreateCartAsync(cart, CancellationToken.None);

        var updatedCart = new Cart(customerId, new Dictionary<Guid, int> { { productId, 3 } });
        await _cartRepository.UpdateCartAsync(updatedCart, CancellationToken.None);

        var cartDb = await _cart.Find(c => c.CustomerId == customerId).FirstOrDefaultAsync(CancellationToken.None);

        cartDb.CustomerId.Should().Be(customerId);
        cartDb.Items.Should().ContainKey(productId);
        cartDb.Items[productId].Should().Be(3);
    }

    [Fact]
    public async Task Get_Cart_Async_Should_Success()
    {
        var customerId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        var cart = new Cart(customerId, new Dictionary<Guid, int> { { productId, 2 } });
        await _cartRepository.CreateCartAsync(cart, CancellationToken.None);

        var cartById = await _cartRepository.GetCartAsync(customerId, CancellationToken.None);

        cartById.CustomerId.Should().Be(cart.CustomerId);
        cartById.Items.Should().ContainKey(productId);
        cartById.Items[productId].Should().Be(2);
    }

    [Fact]
    public async Task Get_Cart_Async_Should_Fail_When_Not_Found()
    {
        var id = Guid.NewGuid();

        var test = async () => await _cartRepository.GetCartAsync(id, CancellationToken.None);

        await test.Should().ThrowAsync<InvalidOperationException>();
    }

    public override void Dispose()
    {
        _context.DeleteDb(_dbName);
    }
}
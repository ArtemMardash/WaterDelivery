using FluentAssertions;
using Mediator;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Backend.Infrastructure.Persistence;
using WaterDelivery.Contracts.Carts.Dtos;
 
namespace WaterDelivery.Tests.IntegrationTests;
 
public class CartUseCases : IAsyncLifetime
{
    private readonly ICartRepository _cartRepository;
    private readonly WaterDeliveryContext _waterDeliveryContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    private readonly IntegrationTestHelper _dbService = new IntegrationTestHelper();
    private static Guid _cartCustomerId;
 
    public CartUseCases()
    {
        _cartRepository = _dbService.GetRequiredService<ICartRepository>();
        _mediator = _dbService.GetRequiredService<IMediator>();
        _unitOfWork = _dbService.GetRequiredService<IUnitOfWork>();
        _waterDeliveryContext = _dbService.GetRequiredService<WaterDeliveryContext>();
        _cartCustomerId = _dbService.CreateCart(_cartRepository, _unitOfWork);
    }
 
    [Fact]
    public async Task Create_Cart_Use_Case_Should_Success()
    {
        var customerId = Guid.NewGuid();
 
        var request = new CreateCartDto
        {
            CustomerId = customerId,
            Items = new Dictionary<Guid, int>()
        };
 
        var result = await _mediator.Send(request, CancellationToken.None);
 
        result.Should().Be(customerId);
    }
 
    [Fact]
    public async Task Update_Cart_Use_Case_Should_Success()
    {
        var customerId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        await _mediator.Send(new CreateCartDto
        {
            CustomerId = customerId,
            Items = new Dictionary<Guid, int>()
        }, CancellationToken.None);

        await _mediator.Send(new UpdateCartDto
        {
            CustomerId = customerId,
            Items = new Dictionary<Guid, int> { { productId, 3 } }
        }, CancellationToken.None);

        var cart = await _cartRepository.GetCartAsync(customerId, CancellationToken.None);

        cart.Should().NotBeNull();
        cart.Items.Should().ContainKey(productId);
        cart.Items[productId].Should().Be(3);
    }
 
    [Fact]
    public async Task Get_Cart_Use_Case_Should_Success()
    {
        var result = await _mediator.Send(new GetCartDto { CustomerId = _cartCustomerId }, CancellationToken.None);
 
        result.CustomerId.Should().Be(_cartCustomerId);
        result.Items.Should().NotBeNull();
        result.TotalPrice.Should().Be(0);
    }
    
    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        _dbService.DeleteDb(_waterDeliveryContext);
        _unitOfWork.Dispose();
    }
}
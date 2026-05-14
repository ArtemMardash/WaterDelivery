using FluentAssertions;
using NSubstitute;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.Carts;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Contracts.Carts.Dtos;

namespace WaterDelivery.Tests.UseCaseTests;

public class CartUseCaseTests
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CartUseCaseTests()
    {
        _cartRepository = Substitute.For<ICartRepository>();
        _productRepository = Substitute.For<IProductRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _unitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
    }

    [Fact]
    public async Task Create_Cart_Should_Success()
    {
        var customerId = Guid.NewGuid();

        var dto = new CreateCartDto
        {
            CustomerId = customerId,
            Items = new Dictionary<Guid, int>()
        };

        _cartRepository.CreateCartAsync(Arg.Any<Cart>(), CancellationToken.None).Returns(customerId); // ← add this

        var useCase = new CreateCartUseCase(_cartRepository, _unitOfWork);
        var result = await useCase.Handle(dto, CancellationToken.None);

        result.Should().Be(customerId);
    }

    [Fact]
    public async Task Get_Cart_Should_Success()
    {
        var customerId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        decimal price = (decimal)5.88;

        var cart = new Cart(customerId, new Dictionary<Guid, int> { { productId, 2 } });

        _cartRepository.GetCartAsync(Arg.Any<Guid>(), CancellationToken.None).Returns(cart);

        _productRepository.GetProductsAsync(Arg.Any<List<Guid>>(), CancellationToken.None)
            .Returns(new List<Product>
            {
                new Product(productId, "Spring Water", "Nice water description here",
                    new List<ProductUnit>(),
                    new ProductUnit(Contracts.Enums.MeasurementUnits.Liter, 1),
                    price,
                    new List<string>())
            });

        var useCase = new GetCartUseCase(_cartRepository, _productRepository, _unitOfWork);
        var result = await useCase.Handle(new GetCartDto { CustomerId = customerId }, CancellationToken.None);

        result.CustomerId.Should().Be(customerId);
        result.Items.Should().ContainKey(productId);
        result.Items[productId].Should().Be(2);
        result.TotalPrice.Should().Be(price * 2);
    }
}
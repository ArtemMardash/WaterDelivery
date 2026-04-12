using FluentAssertions;
using Mediator;
using MongoDB.Driver;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Backend.Infrastructure.Persistence;
using WaterDelivery.Contracts.Enums;
using WaterDelivery.Contracts.Products.Dtos;
using WaterDelivery.Contracts.ProductUnits.Dtos;

namespace WaterDelivery.Tests.IntegrationTests;

public class ProductUseCases: IDisposable
{
    private readonly IProductRepository _productRepository;
    private readonly WaterDeliveryContext _waterDeliveryContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    private readonly IntegrationTestHelper _dbService = new IntegrationTestHelper();
    private static Guid _productId;

    public ProductUseCases()
    {
        _productRepository = _dbService.GetRequiredService<IProductRepository>();
        _mediator = _dbService.GetRequiredService<IMediator>();
        _unitOfWork = _dbService.GetRequiredService<IUnitOfWork>();
        _waterDeliveryContext = _dbService.GetRequiredService<WaterDeliveryContext>();
        _productId = _dbService.CreateProduct(_productRepository, _unitOfWork);
    }

    [Fact]
    public async Task Create_Product_Use_Case_Should_Success()
    {
        var request = new CreateProductDto
        {
            Name = "Water Bottle",
            Description = "Spring water from water fantan",
            ProductOptions = new List<ProductUnitDto>(),
            DefaultUnit = new ProductUnitDto
            {
                Id = Guid.NewGuid(),
                Name = (MeasurementUnits)1,
                QuantityPerUnit = 2
            },
            DefaultUnitPrice = 6
        };

        var result = await _mediator.Send(request, CancellationToken.None);

        result.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public async Task Update_Product_Use_Case_Should_Success()
    {
        var createdId = await _mediator.Send(new CreateProductDto
        {
            Name = "Water Bottle",
            Description = "Spring water from water fantan",
            ProductOptions = new List<ProductUnitDto>(),
            DefaultUnit = new ProductUnitDto
            {
                Id = Guid.NewGuid(),
                Name = (MeasurementUnits)1,
                QuantityPerUnit = 2
            },
            DefaultUnitPrice = 6
        }, CancellationToken.None);

        var update = new UpdateProductDto
        {
            Id = createdId,
            Name = "Water Bottle Updated",
            Description = "Mineral water updated",
            ProductOptions = new List<ProductUnitDto>(),
            DefaultUnit = new ProductUnitDto
            {
                Id = Guid.NewGuid(),
                Name = (MeasurementUnits)1,
                QuantityPerUnit = 2
            },
            DefaultUnitPrice = 7,
        };

        await _mediator.Send(update, CancellationToken.None);

        var product = await _waterDeliveryContext
            .GetCollection<Product>("product")
            .Find(p => p.Id == createdId)
            .FirstOrDefaultAsync();

        product.Should().NotBeNull();
        product.Name.Should().Be("Water Bottle Updated");
        product.Description.Should().Be("Mineral water updated");
        product.ProductOptions.Count.Should().Be(0);
        product.DefaultUnit.Id.Should().Be(update.DefaultUnit.Id);
        product.DefaultUnit.QuantityPerUnit.Should().Be(update.DefaultUnit.QuantityPerUnit);
        product.DefaultUnitPrice.Should().Be(7);
    }

    [Fact]
    public async Task Delete_Product_Use_Case_Should_Success()
    {
        await _mediator.Send(new DeleteProductDto { Id = _productId }, CancellationToken.None);

        var exists = await _waterDeliveryContext
            .GetCollection<Product>("product")
            .Find(p => p.Id == _productId)
            .AnyAsync();

        exists.Should().BeFalse();
    }

    [Fact]
    public async Task Get_Product_Use_Case_Should_Success()
    {
        var result = await _mediator.Send(new GetProductDto { Id = _productId }, CancellationToken.None);

        result.Id.Should().Be(_productId);
        result.Name.Should().Be("productNmae");
        result.Description.Should().Be("productDescription that esdcsdsd");
        result.ProductOptions.Count.Should().Be(0);
        result.DefaultUnit.QuantityPerUnit.Should().Be(2);
        result.DefaultUnitPrice.Should().Be(12);
    }

    public void Dispose()
    {
        _dbService.DeleteDb(_waterDeliveryContext);
        _unitOfWork.Dispose();
    }
}
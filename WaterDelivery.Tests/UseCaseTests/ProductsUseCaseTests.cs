using FluentAssertions;
using NSubstitute;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Core.Enums;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.Products;
using WaterDelivery.Backend.Features.Products.Dtos;
using WaterDelivery.Backend.Features.ProductUnits.Dtos;
using WaterDelivery.Backend.Features.Shared;

namespace WaterDelivery.Tests.UseCaseTests;

public class ProductsUseCaseTests
{
    private readonly IProductRepository _productRepository;

    private readonly IUnitOfWork _unitOfWork;
    
    public ProductsUseCaseTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _unitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
    }

    [Fact]
    public async Task Create_Product_Should_Success()
    {
        var dto = new CreateProductDto
        {
            Name = "apple",
            Description = "fruitttttttttttttttttttttttttttttttttttttttttttttttttttttttttttt",
            ProductOptions = new List<ProductUnitDto>(),
            DefaultUnit = new ProductUnitDto
            {
                Id = Guid.NewGuid(),
                Name = MeasurementUnits.Kilogram,
                QuantityPerUnit = 5
            },
            DefaultUnitPrice = 10
        };

        var useCase = new CreateProductUseCase(_productRepository, _unitOfWork);
        var result = await useCase.Handle(dto, CancellationToken.None);

        result.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Get_Product_Should_Success()
    {
        var product = new Product(
            "apple",
            "fruitttttttttttttttttttttttttttttttttttttttttttttttttttt", 
            new List<ProductUnit>(),
            new ProductUnit(MeasurementUnits.Kilogram, 5), 
            10);

        _productRepository.GetProductByIdAsync(Arg.Any<Guid>(), CancellationToken.None).Returns(product);
        var useCase = new GetProductUseCase(_productRepository);
        var result = await useCase.Handle(new GetProductDto
        {
            Id = Guid.NewGuid()
        }, CancellationToken.None);

        result.Description.Should().Be(product.Description);
        result.Name.Should().Be(product.Name);
        result.DefaultUnit.QuantityPerUnit.Should().Be(product.DefaultUnit.QuantityPerUnit);
        result.DefaultUnit.Name.Should().Be(product.DefaultUnit.Name);
        result.DefaultUnit.Id.Should().Be(product.DefaultUnit.Id);
        result.DefaultUnitPrice.Should().Be(product.DefaultUnitPrice);
        result.Id.Should().Be(product.Id);
    }
}
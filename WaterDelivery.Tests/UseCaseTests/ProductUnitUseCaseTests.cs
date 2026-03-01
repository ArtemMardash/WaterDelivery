using FluentAssertions;
using NSubstitute;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Core.Enums;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.Products;
using WaterDelivery.Backend.Features.Products.Dtos;
using WaterDelivery.Backend.Features.ProductUnits;
using WaterDelivery.Backend.Features.ProductUnits.Dtos;
using WaterDelivery.Backend.Features.Shared;

namespace WaterDelivery.Tests.UseCaseTests;

public class ProductUnitUseCaseTests
{
        private readonly IProductUnitRepository _productUnitRepository;

    private readonly IUnitOfWork _unitOfWork;
    
    public ProductUnitUseCaseTests()
    {
        _productUnitRepository = Substitute.For<IProductUnitRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _unitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
    }

    [Fact]
    public async Task Create_Product_Unit_Should_Success()
    {
        var dto = new CreateProductUnitDto
        {
            Name = MeasurementUnits.Liter,
            QuantityPerUnit = 2
        };

        var useCase = new CreateProductUnitUseCase(_productUnitRepository, _unitOfWork);
        var result = await useCase.Handle(dto, CancellationToken.None);

        result.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Get_Product_Unit_Should_Success()
    {
        var productUnit = new ProductUnit(MeasurementUnits.Liter, 2);

        _productUnitRepository.GetProductUnitAsync(Arg.Any<Guid>(), CancellationToken.None).Returns(productUnit);
        var useCase = new GetProductUnitUseCase(_productUnitRepository);
        var result = await useCase.Handle(new GetProductUnitDto
        {
            Id = Guid.NewGuid()
        }, CancellationToken.None);

        result.Name.Should().Be(productUnit.Name);
        result.QuantityPerUnit.Should().Be(productUnit.QuantityPerUnit);
        result.Id.Should().Be(productUnit.Id);
    }
}
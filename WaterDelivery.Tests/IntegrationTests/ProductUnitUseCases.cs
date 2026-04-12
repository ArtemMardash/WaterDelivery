using FluentAssertions;
using Mediator;
using MongoDB.Driver;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Core.Enums;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.ProductUnits.Dtos;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Backend.Infrastructure.Persistence;

namespace WaterDelivery.Tests.IntegrationTests;

public class ProductUnitUseCases
{
    private readonly IProductUnitRepository _productUnitRepository;
    private readonly WaterDeliveryContext _waterDeliveryContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    private readonly IntegrationTestHelper _dbService = new IntegrationTestHelper();
    private static Guid _productUnitId;

    public ProductUnitUseCases()
    {
        _productUnitRepository = _dbService.GetRequiredService<IProductUnitRepository>();
        _mediator = _dbService.GetRequiredService<IMediator>();
        _unitOfWork = _dbService.GetRequiredService<IUnitOfWork>();
        _waterDeliveryContext = _dbService.GetRequiredService<WaterDeliveryContext>();
        _productUnitId = _dbService.CreatePU(_productUnitRepository, _unitOfWork);
    }

    [Fact]
    public async Task Create_ProductUnit_Use_Case_Should_Success()
    {
        var request = new CreateProductUnitDto
        {
            Name = MeasurementUnits.Gallon,
            QuantityPerUnit = 2
        };

        var result = await _mediator.Send(request, CancellationToken.None);

        result.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public async Task Update_ProductUnit_Use_Case_Should_Success()
    {
        var createdId = await _mediator.Send(new CreateProductUnitDto
        {
            Name = MeasurementUnits.Gallon,
            QuantityPerUnit = 2
        }, CancellationToken.None);

        var update = new UpdateProductUnitDto
        {
            Id = createdId,
            Name = MeasurementUnits.Liter,
            QuantityPerUnit = 5
        };

        await _mediator.Send(update, CancellationToken.None);

        var productUnit = await _waterDeliveryContext
            .GetCollection<ProductUnit>("productUnit")
            .Find(pu => pu.Id == createdId)
            .FirstOrDefaultAsync();

        productUnit.Should().NotBeNull();
        productUnit.Name.Should().Be(MeasurementUnits.Liter);
        productUnit.QuantityPerUnit.Should().Be(5);
    }

    [Fact]
    public async Task Delete_ProductUnit_Use_Case_Should_Success()
    {
        await _mediator.Send(new DeleteProductUnitDto { Id = _productUnitId }, CancellationToken.None);

        var productUnit = await _waterDeliveryContext
            .GetCollection<ProductUnit>("productUnit")
            .Find(pu => pu.Id == _productUnitId)
            .FirstOrDefaultAsync();

        productUnit.Should().BeNull();
    }

    [Fact]
    public async Task Get_ProductUnit_Use_Case_Should_Success()
    {
        var result = await _mediator.Send(new GetProductUnitDto { Id = _productUnitId }, CancellationToken.None);

        result.Id.Should().Be(_productUnitId);
        result.Name.Should().Be(MeasurementUnits.Gallon);
        result.QuantityPerUnit.Should().Be(2);
    }

    public void Dispose()
    {
        _dbService.DeleteDb(_waterDeliveryContext);
        _unitOfWork.Dispose();
    }
}
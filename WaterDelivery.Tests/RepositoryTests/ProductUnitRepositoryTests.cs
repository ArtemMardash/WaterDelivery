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
public class ProductUnitRepositoryTests: BaseTest
{
    private readonly WaterDeliveryContext _context;
    private readonly IProductUnitRepository _productUnitRepository;
    private readonly IMongoCollection<ProductUnitDb> _productUnits;

    private readonly string _dbName = $"productUnitTest_{Guid.NewGuid()}";

    public ProductUnitRepositoryTests()
    {
        _context = new WaterDeliveryContext("mongodb://root:password@localhost:27017/?authSource=admin", _dbName);
        _productUnits = _context.GetCollection<ProductUnitDb>("productUnits");
        _productUnitRepository = new ProductUnitRepository(_context);
    }

    [Fact]
    public async Task Create_Bill_Async_Should_Success()
    {
        var productUnit = new ProductUnit(MeasurementUnits.Gallon, 10);
        var resultId = await _productUnitRepository.CreateProductUnitAsync(productUnit, CancellationToken.None);

        resultId.Should().Be(productUnit.Id);
    }

    [Fact]
    public async Task Update_Bill_Async_Should_Success()
    {
        var productUnit = new ProductUnit(MeasurementUnits.Gallon, 10);
        var resultId = await _productUnitRepository.CreateProductUnitAsync(productUnit, CancellationToken.None);


        var productUnitNew = new ProductUnit(resultId, MeasurementUnits.Liter, 7);

        await _productUnitRepository.UpdateProductUnitAsync(productUnitNew, CancellationToken.None);
        var productUnitAsync = await _productUnitRepository.GetProductUnitAsync(resultId, CancellationToken.None);
        
        productUnitAsync.Id.Should().Be(productUnitNew.Id);
        productUnitAsync.QuantityPerUnit.Should().Be(productUnitNew.QuantityPerUnit);
        productUnitAsync.Name.Should().Be(productUnitNew.Name);
    }

    [Fact]
    public async Task Get_Bill_By_Id_Async_ShouldSuccess()
    {
        var productUnit = new ProductUnit(MeasurementUnits.Gallon, 10);
        var resultId = await _productUnitRepository.CreateProductUnitAsync(productUnit, CancellationToken.None);

        var productUnitById = await _productUnitRepository.GetProductUnitAsync(resultId, CancellationToken.None);

        
        productUnitById.Id.Should().Be(productUnit.Id);
        productUnitById.QuantityPerUnit.Should().Be(productUnit.QuantityPerUnit);
        productUnitById.Name.Should().Be(productUnit.Name);
    }
    
    [Fact]
    public async Task Delete_Bill_Should_Success()
    {
        var productUnit = new ProductUnit(MeasurementUnits.Gallon, 10);
        var resultId = await _productUnitRepository.CreateProductUnitAsync(productUnit, CancellationToken.None);

        await _productUnitRepository.DeleteProductUnitAsync(resultId, CancellationToken.None);

        (await _productUnits.Find(u => u.Id == resultId).FirstOrDefaultAsync(CancellationToken.None)).Should().BeNull();
    }
    
    public override void Dispose()
    {
        _context.DeleteDb(_dbName);
    }
}
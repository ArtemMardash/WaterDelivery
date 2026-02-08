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
public class ProductRepositoryTests: BaseTest
{
    private readonly WaterDeliveryContext _context;
    private readonly IProductRepository _productRepository;
    private readonly IMongoCollection<ProductDb> _product;

    private readonly string _dbName = $"product_test_{Guid.NewGuid()}";

    public ProductRepositoryTests()
    {
        _context = new WaterDeliveryContext("mongodb://root:password@localhost:27017", _dbName);
        _product = _context.GetCollection<ProductDb>("products");
        _productRepository = new ProductRepository(_context);
    }
    

    [Fact]
    public async Task Create_Product_Async_Should_Success()
    {
        var product = new Product("name12", "descriptionjhvvhjkvhkvhvjk", new List<ProductUnit>(),
            new ProductUnit(MeasurementUnits.Kilogram, 2), 21341);
        
        var resultId = await _productRepository.CreateProductAsync(product, CancellationToken.None);

        resultId.Should().Be(product.Id);
    }

    [Fact]
    public async Task Update_Product_Async_Should_Success()
    {
        var product = new Product("name12", "descriptionionjnkljnkljlk", new List<ProductUnit>(),
            new ProductUnit(MeasurementUnits.Kilogram, 2), 21341);
        
        var resultId = await _productRepository.CreateProductAsync(product, CancellationToken.None);


        var productNew = new Product(resultId,"name1", "description1kjbbjblhlkhbj", new List<ProductUnit>(),
            new ProductUnit(MeasurementUnits.Kilogram, 3), 2134);

        await _productRepository.UpdateProductAsync(productNew, CancellationToken.None);
        var productDb = await _productRepository.GetProductByIdAsync(resultId, CancellationToken.None);

        productDb.Name.Should().Be(productNew.Name);
        productDb.Description.Should().Be(productNew.Description);
        productDb.DefaultUnit.QuantityPerUnit.Should().Be(productNew.DefaultUnit.QuantityPerUnit);
        productDb.DefaultUnit.Name.Should().Be(productNew.DefaultUnit.Name);
        productDb.DefaultUnitPrice.Should().Be(productNew.DefaultUnitPrice);
        productDb.Id.Should().Be(productNew.Id);
    }

    [Fact]
    public async Task Get_Product_By_Id_Async_ShouldSuccess()
    {
        var product = new Product("name12", "descriptionkjbhjbhjbk", new List<ProductUnit>(),
            new ProductUnit(MeasurementUnits.Kilogram, 2), 21341);
        
        var resultId = await _productRepository.CreateProductAsync(product, CancellationToken.None);


        var productById = await _productRepository.GetProductByIdAsync(resultId, CancellationToken.None);

        productById.Name.Should().Be(product.Name);
        productById.Description.Should().Be(product.Description);
        productById.DefaultUnit.QuantityPerUnit.Should().Be(product.DefaultUnit.QuantityPerUnit);
        productById.DefaultUnit.Name.Should().Be(product.DefaultUnit.Name);
        productById.DefaultUnitPrice.Should().Be(product.DefaultUnitPrice);
        productById.Id.Should().Be(product.Id);
    }
    
    [Fact]
    public async Task Delete_Product_Should_Success()
    {
        var product = new Product("name12", "descriptionbhjhbkjhbhkj", new List<ProductUnit>(),
            new ProductUnit(MeasurementUnits.Kilogram, 2), 21341);
        
        var resultId = await _productRepository.CreateProductAsync(product, CancellationToken.None);

        (await _product.Find(u => u.Id == resultId).FirstOrDefaultAsync(CancellationToken.None)).Should().BeNull();
    }
    
    public override void Dispose()
    {
        _context.DeleteDb(_dbName);
    }
}
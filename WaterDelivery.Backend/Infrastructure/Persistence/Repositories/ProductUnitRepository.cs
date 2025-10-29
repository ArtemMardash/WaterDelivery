using MongoDB.Driver;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Backend.Infrastructure.Persistence.DbEntities;
using WaterDelivery.Backend.Infrastructure.Persistence.Mapping;

namespace WaterDelivery.Backend.Infrastructure.Persistence.Repositories;

public class ProductUnitRepository: IProductUnitRepository
{
    private readonly IMongoCollection<ProductUnitDb> _productUnit;

    public ProductUnitRepository(WaterDeliveryContext context)
    {
        _productUnit = context.GetCollection<ProductUnitDb>("productUnit");
    }
    public async Task<string> CreateProductUnitAsync(ProductUnit productUnit, CancellationToken cancellationToken)
    {
        var productUnitDb = productUnit.ToDb();
        await _productUnit.InsertOneAsync(productUnitDb, cancellationToken);

        return productUnitDb.Id;
    }

    public async Task UpdateProductUnitAsync(ProductUnit productUnit, CancellationToken cancellationToken)
    {
        await _productUnit.ReplaceOneAsync(productUnit.Id.ToString(), productUnit.ToDb(), cancellationToken: cancellationToken);
    }

    public async Task<ProductUnit> GetProductUnitAsync(Guid id, CancellationToken cancellationToken)
    {
        var productUnitDb = await _productUnit.Find(p => p.Id == id.ToString()).FirstOrDefaultAsync(cancellationToken);

        return productUnitDb.ToDomain();
    }

    public async Task DeleteProductUnitAsync(Guid id, CancellationToken cancellationToken)
    {
        await _productUnit.DeleteOneAsync(p => p.Id == id.ToString(), cancellationToken: cancellationToken);
    }
}
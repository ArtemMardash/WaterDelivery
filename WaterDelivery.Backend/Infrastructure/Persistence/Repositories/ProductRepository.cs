using MongoDB.Driver;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Backend.Infrastructure.Persistence.DbEntities;
using WaterDelivery.Backend.Infrastructure.Persistence.Mapping;

namespace WaterDelivery.Backend.Infrastructure.Persistence.Repositories;

public class ProductRepository: IProductRepository
{
    private readonly IMongoCollection<ProductDb> _product;

    public ProductRepository(WaterDeliveryContext context)
    {
        _product = context.GetCollection<ProductDb>("product");
    }
    
    public async Task<Guid> CreateProductAsync(Product product, CancellationToken cancellationToken)
    {
        var productDb = product.ToDb();
        await _product.InsertOneAsync(productDb, cancellationToken: cancellationToken);

        return productDb.Id;
    }

    public async  Task UpdateProductAsync(Product product, CancellationToken cancellationToken)
    {
        var db = product.ToDb();
        var update = Builders<ProductDb>.Update
            .Set(p => p.Description, db.Description)
            .Set(p => p.DefaultUnit, db.DefaultUnit)
            .Set(p => p.Name, db.Name)
            .Set(p => p.ProductOptions, db.ProductOptions)
            .Set(p => p.Id, db.Id)
            .Set(p => p.DefaultUnitPrice, db.DefaultUnitPrice);
        await _product.UpdateOneAsync(p => p.Id == product.Id, update, cancellationToken: cancellationToken);
    }

    public async Task<Product> GetProductByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var filter = Builders<ProductDb>.Filter.Eq(p => p.Id, id);
        var product = await _product.Find(filter).FirstOrDefaultAsync(cancellationToken);

        return product.ToDomain();
    }

    public async Task DeleteProductAsync(Guid id, CancellationToken cancellationToken)
    {
        await _product.DeleteOneAsync(p => p.Id == id, cancellationToken: cancellationToken);
    }
}
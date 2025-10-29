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
    
    public async Task<string> CreateProductAsync(Product product, CancellationToken cancellationToken)
    {
        var productDb = product.ToDb();
        await _product.InsertOneAsync(productDb, cancellationToken: cancellationToken);

        return productDb.Id;
    }

    public async  Task UpdateProductAsync(Product product, CancellationToken cancellationToken)
    {
        await _product.ReplaceOneAsync(product.Id.ToString(), product.ToDb(), cancellationToken: cancellationToken);
    }

    public async Task<Product> GetProductByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var filter = Builders<ProductDb>.Filter.Eq(p => p.Id, id.ToString());
        var product = await _product.Find(filter).FirstOrDefaultAsync(cancellationToken);

        return product.ToDomain();
    }

    public async Task DeleteProductAsync(Guid id, CancellationToken cancellationToken)
    {
        await _product.DeleteOneAsync(p => p.Id == id.ToString(), cancellationToken: cancellationToken);
    }
}
using WaterDelivery.Backend.Core.Entities;

namespace WaterDelivery.Backend.Features.Shared;

public interface IProductRepository
{
    public Task<string> CreateProductAsync(Product product, CancellationToken cancellationToken);

    public Task UpdateProductAsync(Product product, CancellationToken cancellationToken);

    public Task<Product> GetProductByIdAsync(Guid id, CancellationToken cancellationToken);

    public Task DeleteProductAsync(Guid id, CancellationToken cancellationToken);
}
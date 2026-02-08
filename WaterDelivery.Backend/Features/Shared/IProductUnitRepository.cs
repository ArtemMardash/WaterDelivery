using WaterDelivery.Backend.Core.Entities;

namespace WaterDelivery.Backend.Features.Shared;

public interface IProductUnitRepository
{
    public Task<Guid> CreateProductUnitAsync(ProductUnit productUnit, CancellationToken cancellationToken);

    public Task UpdateProductUnitAsync(ProductUnit productUnit, CancellationToken cancellationToken);

    public Task<ProductUnit> GetProductUnitAsync(Guid id, CancellationToken cancellationToken);

    public Task DeleteProductUnitAsync(Guid id, CancellationToken cancellationToken);

}
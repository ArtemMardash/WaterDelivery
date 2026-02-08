namespace WaterDelivery.Backend.Core.Interfaces;

public interface IUnitOfWork: IDisposable
{
    public void Dispose();

    public void SaveChanges();

    public Task SaveChangesAsync(CancellationToken cancellationToken);
}
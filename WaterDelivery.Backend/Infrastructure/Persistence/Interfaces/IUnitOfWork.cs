namespace WaterDelivery.Backend.Infrastructure.Persistence.Interfaces;

public interface IUnitOfWork: IDisposable
{
    public void Dispose();

    public void SaveChanges();

    public Task SaveChangesAsync();
}
using WaterDelivery.Backend.Core.Interfaces;

namespace WaterDelivery.Backend.Infrastructure.Persistence;

public class UnitOfWork: IUnitOfWork
{
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        
    }

    public void SaveChanges()
    {
        
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
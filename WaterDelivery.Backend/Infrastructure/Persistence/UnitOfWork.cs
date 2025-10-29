using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using MongoDB.Driver;
using WaterDelivery.Backend.Infrastructure.Persistence.Interfaces;

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

    public Task SaveChangesAsync()
    {
        return Task.CompletedTask;
    }
}
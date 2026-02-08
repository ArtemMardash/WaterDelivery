using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Backend.Infrastructure.Persistence;
using WaterDelivery.Backend.Infrastructure.Persistence.Repositories;

namespace WaterDelivery.Backend.Infrastructure;

public static class DependencyInjection
{
    public static void RegisterPersistence(this IServiceCollection services)
    {
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICustomerAddressesRepository, CustomerAddressesRepository>();
        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<IBillRepository, BillRepository>();
        services.AddScoped<IDeliveryRepository, DeliveryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductUnitRepository, ProductUnitRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddSingleton<WaterDeliveryContext>(sp =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var connectionString = config["Mongo:Host"];
            var dbName = config["Mongo:DataBaseName"];
            return new WaterDeliveryContext(connectionString, dbName);
        });
    }
}
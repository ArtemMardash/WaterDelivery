using AspNetCore.Identity.Mongo;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Backend.Infrastructure.Persistence;
using WaterDelivery.Backend.Infrastructure.Persistence.DbEntities;
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
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IOutboxRepository, OutboxRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddSingleton<WaterDeliveryContext>(sp =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var connectionString = config["Mongo:Host"];
            var dbName = config["Mongo:DataBaseName"];
            return new WaterDeliveryContext(connectionString, dbName);
        });

        services.AddIdentityMongoDbProvider<UserDb>(opt =>
        {
            opt.ConnectionString = "mongodb://root:password@localhost:27017/WaterDelivery?authSource=admin";
        });
    }

    public static void RegisterAuth(this IServiceCollection services, IConfiguration config)
    {
        // The Google OAuth handshake now lives in the UI project (see WaterDelivery.UI/Program.cs).
        // The backend intentionally does not register Google authentication or the application cookie
        // anymore: it only upserts users via the /api/auth/google-user endpoint. Kept as a no-op so
        // existing call sites in Program.cs don't need to change.
    }
}
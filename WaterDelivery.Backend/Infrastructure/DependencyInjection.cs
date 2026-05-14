using AspNetCore.Identity.Mongo;
using WaterDelivery.Backend.Core.GoogleAuth;
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
        var externalAuthSettings = config.GetSection(nameof(ExternalAuthSettings)) ??
                                   throw new InvalidOperationException("No external auth section");
        services.ConfigureApplicationCookie(opt =>
        {
            opt.LoginPath = "/login";
            opt.AccessDeniedPath = "/auth-error";
            opt.ExpireTimeSpan = TimeSpan.FromDays(3);
            opt.SlidingExpiration = true;
        });
        services.AddAuthentication().AddGoogle(opt =>
        {
            opt.ClientId = externalAuthSettings["Google:client_id"];
            opt.ClientSecret = externalAuthSettings["Google:client_secret"];
            opt.CallbackPath = "/signin-google";

            opt.AccessType = "offline";
            opt.SaveTokens = true;

            opt.Scope.Add("profile");
            opt.Scope.Add("email");
        });

    }
}
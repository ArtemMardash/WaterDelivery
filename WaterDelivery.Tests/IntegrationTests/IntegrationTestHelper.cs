using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Core.Enums;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Backend.Infrastructure.Persistence;
using WaterDelivery.Backend.Infrastructure.Persistence.Repositories;

namespace WaterDelivery.Tests.IntegrationTests;

public class IntegrationTestHelper
{
    private readonly IServiceProvider _serviceProvider;
    private readonly string _dbName;

    public IntegrationTestHelper()
    {
        var builder = WebApplication.CreateBuilder();
        var services = builder.Services;
        _dbName = $"WaterDelivery_Test{Guid.NewGuid()}";
        
        services.AddSingleton<WaterDeliveryContext>(sp =>
        {
            var connectionString = "mongodb://root:password@localhost:27017";
            var dbName =  _dbName;
            return new WaterDeliveryContext(connectionString, dbName);
        });
        
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICustomerAddressesRepository, CustomerAddressesRepository>();
        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<IBillRepository, BillRepository>();
        services.AddScoped<IDeliveryRepository, DeliveryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductUnitRepository, ProductUnitRepository>();

        try
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        }
        catch( Exception exception)
        {
            Console.WriteLine("Serializer registered");
        }
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        builder.Services.AddMediator(opt=>opt.ServiceLifetime = ServiceLifetime.Scoped);
        
        var scope = builder.Build().Services.CreateScope();

        _serviceProvider = scope.ServiceProvider;
    }
    
    public T GetRequiredService<T>() where T : notnull
    {
        return _serviceProvider.GetRequiredService<T>();
    }

    public Guid CreateUser(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        var user = new User("Artem", UserType.Customer, "artem.m@gmail.com", "9296996892");

        var id = userRepository.CreateUserAsync(user, CancellationToken.None).GetAwaiter().GetResult();
        unitOfWork.SaveChanges();   
        return id;
    }

    public Guid CreateAddress(IAddressRepository addressRepository, IUnitOfWork unitOfWork)
    {
        var address = new Address("Kings Hwy", "10", "5B", "Brooklyn", "NY");

        var id = addressRepository.CreateAddressAsync(address, CancellationToken.None).GetAwaiter().GetResult();
        unitOfWork.SaveChanges();
        return id;
    }

    public void DeleteDb(WaterDeliveryContext waterDeliveryContext)
    {
        waterDeliveryContext.DeleteDb(_dbName);
    }
}
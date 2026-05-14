using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using WaterDelivery.Backend.Core.Interfaces;
using WaterDelivery.Backend.Features.Addresses;
using WaterDelivery.Backend.Features.Bills;
using WaterDelivery.Backend.Features.Carts;
using WaterDelivery.Backend.Features.CustomersAddresses;
using WaterDelivery.Backend.Features.Deliveries;
using WaterDelivery.Backend.Features.GoogleAuth;
using WaterDelivery.Backend.Features.Orders;
using WaterDelivery.Backend.Features.Products;
using WaterDelivery.Backend.Features.ProductUnits;
using WaterDelivery.Backend.Features.S3;
using WaterDelivery.Backend.Features.Users;
using WaterDelivery.Backend.Infrastructure;
using WaterDelivery.Backend.Infrastructure.Persistence.DbEntities;
using WaterDelivery.Backend.Infrastructure.Persistence.S3;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterPersistence();
builder.Services.AddMediator(opt=>opt.ServiceLifetime = ServiceLifetime.Scoped);
builder.Services.AddSingleton<IMinioService, MinioService>();
builder.Services.RegisterAuth(builder.Configuration);


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserDb>>();
    
}
BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();
app.MapAddressEndpoints();
app.MapBillEndpoints();
app.MapCustomerAddressEndpoints();
app.MapOrderEndpoints();
app.MapDeliveryEndpoints();
app.MapProductEndpoints();
app.MapProductUnitEndpoints();
app.MapUserEndpoints();
app.MapS3Endpoints();
app.MapAuthEndpoints();
app.MapCartEndpoints();



app.Run();
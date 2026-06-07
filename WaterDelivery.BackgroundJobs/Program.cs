using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using WaterDelivery.Backend.Infrastructure;
using WaterDelivery.BackgroundJobs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterPersistence();
builder.Services.AddHostedService<DeliveryWorker>();


var app = builder.Build();

BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

app.Run();
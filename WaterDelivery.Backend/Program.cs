using Microsoft.AspNetCore.Mvc;
using WaterDelivery.Backend.Core.Entities;
using WaterDelivery.Backend.Features;
using WaterDelivery.Backend.Features.Shared;
using WaterDelivery.Backend.Infrastructure;
using WaterDelivery.Backend.Infrastructure.Persistence;
using WaterDelivery.Backend.Infrastructure.Persistence.DbEntities;
using WaterDelivery.Backend.Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterPersistence();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.MapPost("user/create", async ([FromBody] CreateUserDto dto, IUserRepository userRepository) =>
    {
        var user = new User(dto.Name, dto.UserType);
        return userRepository.CreateUserAsync(user, CancellationToken.None);
    })
    .WithName("Create")
    .WithTags("User")
    .WithOpenApi();


app.Run();
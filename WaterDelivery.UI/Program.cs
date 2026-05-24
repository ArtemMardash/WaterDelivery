using Microsoft.AspNetCore.Identity;
using WaterDelivery.Backend.Core.GoogleAuth;
using WaterDelivery.Backend.Infrastructure.Persistence.DbEntities;
using WaterDelivery.UI.Components;
using WaterDelivery.UI.Components.Auth;
using WaterDelivery.UI.Components.Cart;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.LoginPath = "/login";
    opt.AccessDeniedPath = "/auth-error";
    opt.ExpireTimeSpan = TimeSpan.FromDays(3);
    opt.SlidingExpiration = true;
});
builder.Services.AddScoped<CartService>();
builder.Services.AddHttpClient("backendClient", c =>
{
    c.BaseAddress = new Uri("http://localhost:5017");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
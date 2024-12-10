using AuthService.Application;
using AuthService.Infrastructure;
using AuthService.Persistence;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://0.0.0.0:8082");

builder.Services
    .AddPersistenceServices(builder.Configuration)
    .AddInfrastructureServices()
    .AddApplicationServices();

var app = builder.Build();

app.UseHttpsRedirection();

app.Run();
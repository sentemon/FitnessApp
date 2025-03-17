using ChatService.Application;
using ChatService.Domain.Constants;
using ChatService.Infrastructure;
using ChatService.Persistence;

var builder = WebApplication.CreateBuilder(args);

var hostingUrl = builder.Configuration[AppSettingsConstants.WebHostUrl];

builder.WebHost.UseUrls(hostingUrl ?? throw new ArgumentNullException(nameof(hostingUrl), "Hosting URL is not configured."));

builder.Services
    .AddPersistenceServices()
    .AddInfrastructureServices()
    .AddApplicationServices();

var app = builder.Build();

app.MapGet("/health", () => Results.Ok("Healthy"));

app.Run();
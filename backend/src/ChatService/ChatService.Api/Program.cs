using ChatService.Application;
using ChatService.Domain.Constants;
using ChatService.Infrastructure;
using ChatService.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var hostingUrl = builder.Configuration[AppSettingsConstants.WebHostUrl];
var connectionString = builder.Configuration[AppSettingsConstants.DatabaseConnectionString];

builder.WebHost.UseUrls(hostingUrl ?? throw new ArgumentNullException(nameof(hostingUrl), "Hosting URL is not configured."));

builder.Services
    .AddPersistenceServices(connectionString)
    .AddInfrastructureServices()
    .AddApplicationServices();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ChatDbContext>();
    dbContext.Database.Migrate();
}

app.MapGet("/health", () => Results.Ok("Healthy"));

app.Run();
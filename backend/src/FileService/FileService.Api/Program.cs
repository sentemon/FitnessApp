using FileService.Application;
using FileService.Infrastructure;
using FileService.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddPersistenceServices(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration)
    .AddApplicationServices();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<FileDbContext>();
    dbContext.Database.Migrate();
}

app.Run();
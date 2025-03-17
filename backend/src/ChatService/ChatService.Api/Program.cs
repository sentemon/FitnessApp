using ChatService.Application;
using ChatService.Infrastructure;
using ChatService.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddPersistenceServices()
    .AddInfrastructureServices()
    .AddApplicationServices();

var app = builder.Build();

app.Run();
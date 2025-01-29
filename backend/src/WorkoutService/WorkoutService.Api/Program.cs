using WorkoutService.Api.GraphQL;
using WorkoutService.Application;
using WorkoutService.Infrastructure;
using WorkoutService.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddPersistenceServices()
    .AddInfrastructureServices()
    .AddApplicationServices();

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>();

builder.Services.AddGraphQL();

var app = builder.Build();

// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/health", () => Results.Ok("Healthy"));

app.MapGraphQL();

app.Run();
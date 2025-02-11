using Microsoft.EntityFrameworkCore;
using WorkoutService.Api.GraphQL;
using WorkoutService.Application;
using WorkoutService.Domain.Constants;
using WorkoutService.Infrastructure;
using WorkoutService.Persistence;

var builder = WebApplication.CreateBuilder(args);

var hostingUrl = builder.Configuration[AppSettingsConstants.WebHostUrl];
var connectionString = builder.Configuration[AppSettingsConstants.DatabaseConnectionString];

builder.WebHost.UseUrls(hostingUrl ?? throw new ArgumentNullException(nameof(hostingUrl), "Hosting URL is not configured."));

builder.Services.AddHttpContextAccessor();

builder.Services
    .AddPersistenceServices(connectionString)
    .AddInfrastructureServices(builder.Configuration)
    .AddApplicationServices();

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>();

builder.Services.AddGraphQL();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<WorkoutDbContext>();
    dbContext.Database.Migrate();
}

// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/health", () => Results.Ok("Healthy"));

app.MapGraphQL();

app.Run();
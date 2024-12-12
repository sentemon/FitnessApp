using Microsoft.EntityFrameworkCore;
using PostService.Api.GraphQL.Mutations;
using PostService.Api.GraphQL.Queries;
using PostService.Application;
using PostService.Domain.Constants;
using PostService.Infrastructure;
using PostService.Persistence;

var builder = WebApplication.CreateBuilder(args);

var hostingUrl = builder.Configuration[AppSettingsConstants.WebHostUrl];

var connectionString = builder.Configuration[AppSettingsConstants.DatabaseConnectionString];

builder.WebHost.UseUrls(hostingUrl ?? throw new ArgumentNullException(nameof(hostingUrl), "Hosting URL is not configured."));

builder.Services
    .AddPersistenceServices(connectionString)
    .AddInfrastructureServices()
    .AddApplicationServices();

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddType(new UuidType());

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PostDbContext>();
    dbContext.Database.Migrate();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// app.UseAuthentication();
// app.UseAuthorization();

app.MapGet("/health", () => Results.Ok("Healthy"));

app.MapGraphQL();

app.Run();
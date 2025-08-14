using AuthService.Api.GraphQL;
using AuthService.Application;
using AuthService.Domain.Constants;
using AuthService.Infrastructure;
using AuthService.Persistence;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Shared.Application.Extensions;

var builder = WebApplication.CreateBuilder(args);

var hostingUrl = builder.Configuration[AppSettingsConstants.WebHostUrl];
var connectionString = builder.Configuration[AppSettingsConstants.DatabaseConnectionString];

builder.WebHost.UseUrls(hostingUrl ?? throw new ArgumentNullException(nameof(hostingUrl), "Hosting URL is not configured."));

builder.Services.AddHttpContextAccessor();

builder.Services
    .AddPersistenceServices(connectionString)
    .AddInfrastructureServices(builder.Configuration)
    .AddApplicationServices(builder.Configuration);

builder.Services
    .AddGraphQLServer()
    .ModifyRequestOptions(options => options.IncludeExceptionDetails = true)
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddType(new UuidType())
    .AddType(new UnsignedIntType());
    // .AddAuthorization();

builder.Services.AddGraphQL();

builder.Services.ConfigureSerilog(builder.Configuration);
builder.Host.UseSerilog();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
    dbContext.Database.Migrate();
}

// app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/health", () => Results.Ok("Healthy"));

app.MapGraphQL();

app.Run();
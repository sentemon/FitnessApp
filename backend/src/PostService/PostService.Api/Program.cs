using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using PostService.Api.GraphQL;
using PostService.Application;
using PostService.Domain.Constants;
using PostService.Infrastructure;
using PostService.Persistence;
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
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddType(new UuidType())
    .AddType<UnsignedIntType>()
    .AddType<UploadType>();

builder.Services.AddGraphQL();

builder.Services.ConfigureSerilog(builder.Configuration);
builder.Host.UseSerilog();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PostDbContext>();
    dbContext.Database.Migrate();
}


app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    // ReSharper disable RedundantEmptyObjectOrCollectionInitializer
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
    KnownNetworks = { },
    KnownProxies = { }
});

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/health", () => Results.Ok("Healthy"));

app.MapGraphQL();

app.Run();
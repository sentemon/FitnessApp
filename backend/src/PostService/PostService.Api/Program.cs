using Microsoft.EntityFrameworkCore;
using PostService.Api.GraphQL;
using PostService.Application;
using PostService.Domain.Constants;
using PostService.Infrastructure;
using PostService.Persistence;

var builder = WebApplication.CreateBuilder(args);

var allowedOrigins = builder.Configuration.GetSection(AppSettingsConstants.AllowedOrigins).Get<string[]>();
var hostingUrl = builder.Configuration[AppSettingsConstants.WebHostUrl];
var connectionString = builder.Configuration[AppSettingsConstants.DatabaseConnectionString];

builder.WebHost.UseUrls(hostingUrl ?? throw new ArgumentNullException(nameof(hostingUrl), "Hosting URL is not configured."));

builder.Services
    .AddCors(options =>
    {
        options.AddPolicy("CorsPolicy", policyBuilder =>
        {
            policyBuilder
                .WithOrigins(allowedOrigins ?? throw new ArgumentNullException(nameof(allowedOrigins),
                    "Allowed Origin URLs are not configured."))
                .AllowCredentials()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    });

builder.Services.AddHttpContextAccessor();

builder.Services
    .AddPersistenceServices(connectionString)
    .AddInfrastructureServices()
    .AddApplicationServices(builder.Configuration);

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddType(new UuidType())
    .AddType<UnsignedIntType>()
    .AddType<UploadType>();

builder.Services.AddGraphQL();

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

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/health", () => Results.Ok("Healthy"));

app.MapGraphQL();

app.Run();
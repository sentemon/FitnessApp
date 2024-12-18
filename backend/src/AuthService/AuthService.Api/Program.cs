using AuthService.Application;
using AuthService.Domain.Constants;
using AuthService.Infrastructure;
using AuthService.Persistence;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var hostingUrl = builder.Configuration[AppSettingsConstants.WebHostUrl];
var connectionString = builder.Configuration[AppSettingsConstants.DatabaseConnectionString];
var allowedOrigins = builder.Configuration.GetSection(AppSettingsConstants.AllowedOrigins).Get<string[]>();

builder.WebHost.UseUrls(hostingUrl ?? throw new ArgumentNullException(nameof(hostingUrl), "Hosting URL is not configured."));

// ToDo: move to extension method
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

builder.Services
    .AddPersistenceServices(connectionString)
    .AddInfrastructureServices(builder.Configuration)
    .AddApplicationServices();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
    dbContext.Database.Migrate();
}

app.UseMiddleware<ExceptionHandlerMiddleware>();

// app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.Run();
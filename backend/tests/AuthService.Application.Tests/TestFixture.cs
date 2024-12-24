using AuthService.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.Keycloak;
using Testcontainers.PostgreSql;

namespace AuthService.Application.Tests;

public class TestFixture
{
    public readonly AuthDbContext AuthDbContextFixture;
    
    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder()
        .WithImage("postgres:15-alpine")
        .Build();

    private readonly KeycloakContainer _keycloakContainer= new KeycloakBuilder()
        .WithImage("quay.io/keycloak/keycloak:26.0")
        .WithPortBinding(8080, true)
        .WithEnvironment("KEYCLOAK_USER", "admin")
        .WithEnvironment("KEYCLOAK_PASSWORD", "admin")
        .Build();

    public TestFixture()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();
        
        _postgreSqlContainer.StartAsync().Wait();
        _keycloakContainer.StartAsync().Wait();

        var connectionString = _postgreSqlContainer.GetConnectionString();
        var serviceProvider = TestStartup.Initialize(connectionString, configuration);

        AuthDbContextFixture = serviceProvider.GetRequiredService<AuthDbContext>();
        ApplyMigrations();
        
        
    }
    
    private void ApplyMigrations()
    {
        using var scope = AuthDbContextFixture.Database.BeginTransaction();
        try
        {
            AuthDbContextFixture.Database.Migrate();
            scope.Commit();
        }
        catch (Exception)
        {
            scope.Rollback();
            throw;
        }
    }
}
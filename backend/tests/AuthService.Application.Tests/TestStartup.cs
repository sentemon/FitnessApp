using AuthService.Infrastructure;
using AuthService.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.Application.Tests;

public class TestStartup
{
    public static ServiceProvider Initialize(string connectionString, IConfiguration configuration)
    {
        var services = new ServiceCollection();
        
        services
            .AddPersistenceServices(connectionString)
            .AddInfrastructureServices(configuration)
            .AddApplicationServices();
            
        return services.BuildServiceProvider();
    }
}
using FileService.Domain.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FileService.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseConnectionString = configuration.GetConnectionString(AppSettingsConstants.DatabaseConnectionString);
        
        if (string.IsNullOrEmpty(databaseConnectionString))
        {
            throw new ArgumentNullException(nameof(databaseConnectionString), "Connection String cannot be empty.");
        }
        
        services.AddDbContext<FileDbContext>(options =>
            options.UseNpgsql(databaseConnectionString));
        return services;
    }
}
using Microsoft.Extensions.DependencyInjection;

namespace WorkoutService.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, string? connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentNullException(nameof(connectionString), "Connection String cannot be empty.");
        }
        
        return services;
    }
}
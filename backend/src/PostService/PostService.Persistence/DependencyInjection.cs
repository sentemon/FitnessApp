using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace PostService.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, string? connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentNullException(nameof(connectionString), "Connection String cannot be empty.");
        }
        
        services.AddDbContext<PostDbContext>(options =>
            options.UseNpgsql(connectionString));
        
        return services;
    }
}
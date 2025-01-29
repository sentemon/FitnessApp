using Microsoft.Extensions.DependencyInjection;

namespace WorkoutService.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
    {
        return services;
    }
}
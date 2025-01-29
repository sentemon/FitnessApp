using Microsoft.Extensions.DependencyInjection;

namespace WorkoutService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        return services;
    }
}
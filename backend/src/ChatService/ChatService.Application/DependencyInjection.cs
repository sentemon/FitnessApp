using Microsoft.Extensions.DependencyInjection;

namespace ChatService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        return services;
    }
}
using Microsoft.Extensions.DependencyInjection;

namespace ChatService.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
    {
        return services;
    }
}
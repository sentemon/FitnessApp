using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Extensions;

namespace ChatService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddQueryHandlers(Assembly.GetExecutingAssembly());
        services.AddCommandHandlers(Assembly.GetExecutingAssembly());
        
        return services;
    }
}
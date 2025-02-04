using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Abstractions;

namespace Shared.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddCommandHandlers(this IServiceCollection services, Assembly assembly)
    {
        var commandHandlerInterface = typeof(ICommandHandler<,>);

        var handlers = assembly.GetTypes()
            .Where(t => t.GetInterfaces()
                .Any(i => i.GetGenericTypeDefinition() == commandHandlerInterface));

        foreach (var handler in handlers)
        {
            services.AddScoped(handler);
        }
    }

    public static void AddQueryHandlers(this IServiceCollection services, Assembly assembly)
    {
        var queryHandlerInterface = typeof(IQueryHandler<,>);

        var handlers = assembly.GetTypes()
            .Where(t => t.GetInterfaces()
                .Any(i => i.GetGenericTypeDefinition() == queryHandlerInterface));

        foreach (var handler in handlers)
        {
            services.AddScoped(handler);
        }
    }
}
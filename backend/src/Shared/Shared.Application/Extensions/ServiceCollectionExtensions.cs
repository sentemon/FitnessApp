using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Shared.Application.Abstractions;

namespace Shared.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddCommandHandlers(this IServiceCollection services, Assembly assembly)
    {
        var commandHandlerInterface = typeof(ICommandHandler<,>);

        var handlers = assembly.GetTypes()
            .Where(t => t.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == commandHandlerInterface));

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
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == queryHandlerInterface));

        foreach (var handler in handlers)
        {
            services.AddScoped(handler);
        }
    }
    
    public static void ConfigureSerilog(this IServiceCollection services, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .Enrich.WithEnvironmentName()
            .Enrich.WithThreadId()
            .WriteTo.Console()
            .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
            .WriteTo.Seq("http://seq:5341")
            .CreateLogger();
    }
}
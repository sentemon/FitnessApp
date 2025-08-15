using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Shared.Application.Abstractions;
using Shared.Application.Constants;

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
    
    public static void ConfigureSerilog(this IServiceCollection _, IConfiguration configuration)
    {
        var seqUrl = configuration[AppSettingsConstants.SeqUrl] ?? throw new ArgumentException("Seq URL is not configured.");

        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithEnvironmentName()
            .Enrich.WithThreadId()
            .WriteTo.Console()
            .WriteTo.File(
                path: "../logs/log-.txt",
                rollingInterval: RollingInterval.Day,
                restrictedToMinimumLevel: LogEventLevel.Warning)
            .WriteTo.Seq(seqUrl)
            .CreateLogger();
    }
}
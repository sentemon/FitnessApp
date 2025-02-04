using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Extensions;

namespace WorkoutService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        
        services.AddCommandHandlers(Assembly.GetExecutingAssembly());
        services.AddQueryHandlers(Assembly.GetExecutingAssembly());
        
        return services;
    }
}
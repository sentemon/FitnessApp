using System.Reflection;
using FluentValidation;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Extensions;
using WorkoutService.Application.Consumers;
using WorkoutService.Domain.Constants;

namespace WorkoutService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        
        services.AddCommandHandlers(Assembly.GetExecutingAssembly());
        services.AddQueryHandlers(Assembly.GetExecutingAssembly());
        
        var rabbitMqHost = configuration[AppSettingsConstants.RabbitMqHost] ?? throw new ArgumentException("RabbitMQ Host is not configured.");
        var rabbitMqUsername = configuration[AppSettingsConstants.RabbitMqUsername] ?? throw new ArgumentException("RabbitMQ Username is not configured.");
        var rabbitMqPassword = configuration[AppSettingsConstants.RabbitMqPassword] ?? throw new ArgumentException("RabbitMQ Password is not configured.");

        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.AddConsumer<UserCreatedEventConsumer>();
            
            busConfigurator.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(rabbitMqHost, host =>
                {
                    host.Username(rabbitMqUsername);
                    host.Password(rabbitMqPassword);
                });
                
                configurator.UseMessageRetry(r => r.Immediate(5));
                
                configurator.ConfigureEndpoints(context); // ToDo: fix
            });
        });
        
        return services;
    }
}
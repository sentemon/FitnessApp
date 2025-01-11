using FileService.Application.Commands.DownloadPost;
using FileService.Application.Commands.UploadPost;
using FileService.Application.Consumers;
using FileService.Domain.Constants;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FileService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<DownloadPostCommandHandler>();
        services.AddScoped<UploadPostCommandHandler>();
        
        var rabbitMqHost = configuration[AppSettingsConstants.RabbitMqHost] ?? throw new ArgumentException("RabbitMQ Host is not configured.");
        var rabbitMqUsername = configuration[AppSettingsConstants.RabbitMqUsername] ?? throw new ArgumentException("RabbitMQ Username is not configured.");
        var rabbitMqPassword = configuration[AppSettingsConstants.RabbitMqPassword] ?? throw new ArgumentException("RabbitMQ Password is not configured.");
        
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.AddConsumer<PostUploadEventConsumer>();
            
            busConfigurator.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(rabbitMqHost, host =>
                {
                    host.Username(rabbitMqUsername);
                    host.Password(rabbitMqPassword);
                });
                
                configurator.ConfigureEndpoints(context);
            });
        });
        
        return services;
    }
}
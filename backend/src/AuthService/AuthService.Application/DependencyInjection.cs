using AuthService.Application.Commands.Login;
using AuthService.Application.Commands.Logout;
using AuthService.Application.Commands.Register;
using AuthService.Application.Commands.ResetPassword;
using AuthService.Application.Commands.SendVerifyEmail;
using AuthService.Application.Commands.UpdateUser;
using AuthService.Application.Commands.VerifyEmail;
using AuthService.Application.Queries.GetUserById;
using AuthService.Application.Queries.GetUserByUsername;
using AuthService.Domain.Constants;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<LoginCommandHandler>();
        services.AddScoped<LogoutCommandHandler>();
        services.AddScoped<RegisterCommandHandler>();
        services.AddScoped<ResetPasswordCommandHandler>();
        services.AddScoped<SendVerifyEmailCommandHandler>();
        services.AddScoped<UpdateUserCommandHandler>();
        services.AddScoped<VerifyEmailCommandHandler>();
        services.AddScoped<GetUserByIdQueryHandler>();
        services.AddScoped<GetUserByUsernameQueryHandler>();

        var rabbitMqHost = configuration[AppSettingsConstants.RabbitMqHost] ?? throw new ArgumentException("RabbitMQ Host is not configured.");
        var rabbitMqUsername = configuration[AppSettingsConstants.RabbitMqUsername] ?? throw new ArgumentException("RabbitMQ Username is not configured.");
        var rabbitMqPassword = configuration[AppSettingsConstants.RabbitMqPassword] ?? throw new ArgumentException("RabbitMQ Password is not configured.");
        
        services.AddMassTransit(busConfigurator =>
        {
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
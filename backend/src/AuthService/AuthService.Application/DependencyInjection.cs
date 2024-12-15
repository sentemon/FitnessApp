using AuthService.Application.Commands.Login;
using AuthService.Application.Commands.Logout;
using AuthService.Application.Commands.Register;
using AuthService.Application.Commands.UpdateUser;
using AuthService.Application.Queries.GetUserById;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<RegisterCommandHandler>();
        services.AddScoped<LoginCommandHandler>();
        services.AddScoped<LogoutCommandHandler>();
        services.AddScoped<UpdateUserCommandHandler>();
        services.AddScoped<GetUserByIdQueryHandler>();
        
        return services;
    }
}
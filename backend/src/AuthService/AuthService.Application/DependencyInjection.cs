using AuthService.Application.Commands.Register;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<RegisterCommandHandler>();
        
        return services;
    }
}
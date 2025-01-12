using AuthService.Domain.Constants;
using AuthService.Infrastructure.Configurations;
using AuthService.Infrastructure.Interfaces;
using AuthService.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Authentication;

namespace AuthService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var keycloakSection = configuration.GetSection(AppSettingsConstants.Keycloak);
        
        services.Configure<KeycloakConfig>(keycloakSection);
        
        var keycloakConfig = new KeycloakConfig(
            keycloakSection[AppSettingsConstants.KeycloakUrl],
            keycloakSection[AppSettingsConstants.KeycloakRealm],
            keycloakSection[AppSettingsConstants.KeycloakClientId],
            keycloakSection[AppSettingsConstants.KeycloakClientSecret],
            keycloakSection[AppSettingsConstants.AdminUsername],
            keycloakSection[AppSettingsConstants.AdminPassword]
        );

        services.AddSingleton(keycloakConfig);

        services.AddHttpClient("KeycloakClient", client =>
        {
            client.BaseAddress = new Uri(keycloakConfig.Url + "/");
        });

        services.AddScoped<IAuthService, Services.AuthService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserService, UserService>();

        services.AddCustomAuthentication(configuration);
        services.AddAuthorization();
        
        return services;
    }
}
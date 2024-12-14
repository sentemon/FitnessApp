using AuthService.Domain.Constants;
using AuthService.Infrastructure.Interfaces;
using AuthService.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfigurationSection keycloakConfig)
    {
        var keycloakUrl = keycloakConfig[AppSettingsConstants.KeycloakUrl] ?? throw new ArgumentNullException(nameof(keycloakConfig), "Keycloak URL is not configured.");
        var keycloakRealm = keycloakConfig[AppSettingsConstants.KeycloakRealm] ?? throw new ArgumentNullException(nameof(keycloakConfig), "Keycloak Realm is not configured.");
        var keycloakClientId = keycloakConfig[AppSettingsConstants.KeycloakClientId] ?? throw new ArgumentNullException(nameof(keycloakConfig), "Keycloak ClientId is not configured.");
        var keycloakClientSecret = keycloakConfig[AppSettingsConstants.KeycloakClientSecret] ?? throw new ArgumentNullException(nameof(keycloakConfig), "Keycloak Client Secret is not configured");
        var keycloakAdminUsername = keycloakConfig[AppSettingsConstants.AdminUsername] ?? throw new ArgumentNullException(nameof(keycloakConfig),"Keycloak Admin Username is not configured.");
        var keycloakAdminPassword = keycloakConfig[AppSettingsConstants.AdminPassword] ?? throw new ArgumentNullException(nameof(keycloakConfig), "Keycloak Admin Password is not configured.");

        services.AddHttpClient<IKeycloakService, KeycloakService>(client =>
        {
            client.BaseAddress = new Uri(keycloakUrl);
            
            return new KeycloakService(client, keycloakRealm, keycloakClientId, keycloakClientSecret, keycloakAdminUsername, keycloakAdminPassword);
        });

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = $"{keycloakUrl}/realms/{keycloakRealm}";
                options.Audience = keycloakClientId;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = $"{keycloakUrl}/realms/{keycloakRealm}",
                    ValidateAudience = true,
                    ValidAudience = keycloakClientId,
                    ValidateLifetime = true
                };

                options.RequireHttpsMetadata = false;
            });
        
        return services;
    }
}
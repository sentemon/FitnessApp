using AuthService.Domain.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfigurationSection keycloakConfig)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority =
                    $"{keycloakConfig[AppSettingsConstants.KeycloakUrl]}/realms/{keycloakConfig[AppSettingsConstants.KeycloakRealm]}";
                options.Audience = keycloakConfig[AppSettingsConstants.KeycloakClientId];
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer =
                        $"{keycloakConfig[AppSettingsConstants.KeycloakUrl]}/realms/{keycloakConfig[AppSettingsConstants.KeycloakRealm]}",
                    ValidateAudience = true,
                    ValidAudience = keycloakConfig[AppSettingsConstants.KeycloakClientId],
                    ValidateLifetime = true
                };

                options.RequireHttpsMetadata = false;
            });
        
        return services;
    }
}
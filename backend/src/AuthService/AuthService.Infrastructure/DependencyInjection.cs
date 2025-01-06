using System.Security.Cryptography;
using System.Text.Json;
using AuthService.Domain.Constants;
using AuthService.Infrastructure.Configurations;
using AuthService.Infrastructure.Interfaces;
using AuthService.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

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

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = $"{keycloakConfig.Url}/realms/{keycloakConfig.Realm}";
                options.Audience = "account";
                options.RequireHttpsMetadata = false;
                options.MetadataAddress = $"{keycloakConfig.Url}/realms/fitness-app-realm/.well-known/openid-configuration";
                options.IncludeErrorDetails = true;
                
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = $"http://localhost:8080/realms/{keycloakConfig.Realm}",
                    ValidateAudience = true,
                    ValidAudience = "account",
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = false,
                    SignatureValidator = (token, parameters) =>
                    {
                        var jwt = new JsonWebToken(token);
                        if (parameters.ValidateIssuer && parameters.ValidIssuer != jwt.Issuer)
                        {
                            return null;
                        }

                        return jwt;
                    }
                };
            });
        services.AddAuthorization();
        
        return services;
    }
}
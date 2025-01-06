using System.Security.Cryptography;
using System.Text.Json;
using AuthService.Domain.Constants;
using AuthService.Infrastructure.Configurations;
using AuthService.Infrastructure.Interfaces;
using AuthService.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            keycloakSection[AppSettingsConstants.KeycloakClientUuid],
            keycloakSection[AppSettingsConstants.KeycloakClientSecret],
            keycloakSection[AppSettingsConstants.AdminUsername],
            keycloakSection[AppSettingsConstants.AdminPassword]
        );

        var rsaSecurityKey = GetRsaSecurityKeyFromKeycloak(keycloakConfig.Url, keycloakConfig.Realm);

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

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = $"{keycloakConfig.Url}/realms/{keycloakConfig.Realm}",
                    ValidateAudience = true,
                    ValidAudience = "account",
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = rsaSecurityKey
                };

                options.RequireHttpsMetadata = false;
            });
        services.AddAuthorization();
        
        return services;
    }
    
    private static RsaSecurityKey GetRsaSecurityKeyFromKeycloak(string keycloakUrl, string realm)
    {
        using var httpClient = new HttpClient();
        var certsUrl = $"{keycloakUrl}/realms/{realm}/protocol/openid-connect/certs";
        var response = httpClient.GetStringAsync(certsUrl).Result;

        var jwks = JsonDocument.Parse(response).RootElement;
        var key = jwks.GetProperty("keys")[0];
        
        var modulusBase64 = key.GetProperty("n").GetString()?.Trim();
        var exponentBase64 = key.GetProperty("e").GetString()?.Trim();
        
        if (string.IsNullOrEmpty(modulusBase64) || string.IsNullOrEmpty(exponentBase64))
        {
            throw new FormatException("Invalid modulus or exponent in the public key");
        }

        try
        {
            modulusBase64 = ConvertUrlBase64ToStandardBase64(modulusBase64);
            exponentBase64 = ConvertUrlBase64ToStandardBase64(exponentBase64);

            var modulus = Convert.FromBase64String(modulusBase64);
            var exponent = Convert.FromBase64String(exponentBase64);

            return new RsaSecurityKey(new RSAParameters
            {
                Modulus = modulus,
                Exponent = exponent
            });
        }
        catch (FormatException ex)
        {
            throw new FormatException("Base64 decoding failed for modulus or exponent", ex);
        }
    }

    private static string ConvertUrlBase64ToStandardBase64(string urlBase64)
    {
        return urlBase64.Replace('-', '+').Replace('_', '/') + new string('=', (4 - urlBase64.Length % 4) % 4);
    }
}
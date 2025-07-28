using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Shared.Authentication;

public static class AuthenticationExtensions
{
    public static void AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var keycloakSection = configuration.GetSection(AuthenticationConstants.KeycloakSection);
        
        services.Configure<AuthenticationConfig>(keycloakSection);
        
        var keycloakConfig = new AuthenticationConfig(
            keycloakSection[AuthenticationConstants.KeycloakUrl],
            keycloakSection[AuthenticationConstants.KeycloakRealm],
            keycloakSection[AuthenticationConstants.KeycloakAudience]
        );

        services.AddSingleton(keycloakConfig);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.Authority = keycloakConfig.Authority;
            options.Audience = keycloakConfig.Audience;
            options.RequireHttpsMetadata = false;
            options.MetadataAddress = keycloakConfig.MetadataAddress;
            options.IncludeErrorDetails = true;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = keycloakConfig.ValidIssuer,
                ValidateAudience = true,
                ValidAudience = keycloakConfig.Audience,
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
            
            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    Console.WriteLine("Token validation failed: " + context.Exception);
                    return Task.CompletedTask;
                },
                OnMessageReceived = context =>
                {
                    if (context.Request.Cookies.ContainsKey("token"))
                    {
                        context.Token = context.Request.Cookies["token"];
                    }
                    return Task.CompletedTask;
                }
            };

        });
    }
}
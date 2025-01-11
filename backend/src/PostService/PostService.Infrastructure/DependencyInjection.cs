using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace PostService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        // ToDo: change hard code (move to shared)
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = $"http://localhost:8080/realms/fitness-app-realm";
                options.Audience = "account";
                options.RequireHttpsMetadata = false;
                options.MetadataAddress = $"http://localhost:8080/realms/fitness-app-realm/.well-known/openid-configuration";
                options.IncludeErrorDetails = true;
                
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = $"http://localhost:8080/realms/fitness-app-realm",
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
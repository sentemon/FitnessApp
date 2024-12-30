using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace PostService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        var rsaSecurityKey = new RsaSecurityKey(
            new RSAParameters
            {
                Modulus = Convert.FromBase64String("MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA1Q/E+u/w56fl1C67jv2glu/wjy50re4UbVqJHX+T84hN8lGjiHRxqmEgdsyArhiXK0XBbbolPJ+jLTXY7KAKXXtsCjNOV/WLCKFi9Gdn2vgrmD+g2BatpRvO33nytwlDZSkZSnFpTY3io6ZcMCB+YQK5i+2QOz1gahJt5Dac+bwZ96d3x+dEP+0lZ3+3VTe0bKWRMDked2f8E4K9mvywyUEf3Ihe/2YVhXZyUnshMrRmn6ZZ4DvpQcrHMtGMCBiB5N6pllxBu5XAcxYPRxZtx+q4GSU1bCA9es4RNgLTBGP8GQXVVqlq80j8oxR1SSigqLNbA7qcGi4+rmWg/ohdkQIDAQAB"),
                Exponent = Convert.FromBase64String("AQAB")
            }
        );
        
        // ToDo: change hard code (move to shared)
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = "http://localhost:8080/realms/fitness-app-realm";
                options.Audience = "account";

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = "http://localhost:8080/realms/fitness-app-realm",
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
}
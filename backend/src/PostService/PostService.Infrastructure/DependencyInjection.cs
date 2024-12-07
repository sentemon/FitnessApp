using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace PostService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        // ToDo: change hard code
        // services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //     .AddJwtBearer(options =>
        //     {
        //         options.Authority = "http://localhost:8080/realms/fitness-app-realm/";
        //         options.Audience = "fitness-app-client";
        //         options.RequireHttpsMetadata = true;
        //         options.TokenValidationParameters = new TokenValidationParameters
        //         {
        //             ValidateIssuer = true,
        //             ValidateAudience = true,
        //             ValidateLifetime = true,
        //             ValidIssuer = "http://localhost:8080/realms/fitness-app-realm/",
        //             ValidAudience = "fitness-app-client"
        //         };
        //     });
        //
        // services.AddAuthorization();
        
        return services;
    }
}
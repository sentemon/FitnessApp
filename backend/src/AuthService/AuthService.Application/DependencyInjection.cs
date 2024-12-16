using AuthService.Application.Commands.Login;
using AuthService.Application.Commands.Logout;
using AuthService.Application.Commands.Register;
using AuthService.Application.Commands.ResetPassword;
using AuthService.Application.Commands.SendVerifyEmail;
using AuthService.Application.Commands.UpdateUser;
using AuthService.Application.Commands.VerifyEmail;
using AuthService.Application.Queries.GetUserById;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<LoginCommandHandler>();
        services.AddScoped<LogoutCommandHandler>();
        services.AddScoped<RegisterCommandHandler>();
        services.AddScoped<ResetPasswordCommandHandler>();
        services.AddScoped<SendVerifyEmailCommandHandler>();
        services.AddScoped<UpdateUserCommandHandler>();
        services.AddScoped<VerifyEmailCommandHandler>();
        services.AddScoped<GetUserByIdQueryHandler>();
        
        return services;
    }
}
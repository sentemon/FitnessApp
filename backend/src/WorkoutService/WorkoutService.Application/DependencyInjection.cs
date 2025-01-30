using Microsoft.Extensions.DependencyInjection;
using WorkoutService.Application.Commands.CreateWorkout;
using WorkoutService.Application.Commands.DeleteWorkout;

namespace WorkoutService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<CreateWorkoutCommandHandler>();
        services.AddScoped<DeleteWorkoutCommandHandler>();
        
        return services;
    }
}
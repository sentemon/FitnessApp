using Microsoft.Extensions.DependencyInjection;
using WorkoutService.Application.Commands.CreateWorkout;
using WorkoutService.Application.Commands.DeleteWorkout;
using WorkoutService.Application.Commands.UpdateWorkout;

namespace WorkoutService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<CreateWorkoutCommandHandler>();
        services.AddScoped<UpdateWorkoutCommandHandler>();
        services.AddScoped<DeleteWorkoutCommandHandler>();
        
        return services;
    }
}
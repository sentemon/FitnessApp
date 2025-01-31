using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using WorkoutService.Application.Commands.CreateWorkout;
using WorkoutService.Application.Commands.DeleteWorkout;
using WorkoutService.Application.Commands.MarkSetAsCompleted;
using WorkoutService.Application.Commands.MarkSetAsUncompleted;
using WorkoutService.Application.Commands.UpdateWorkout;

namespace WorkoutService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        
        services.AddScoped<CreateWorkoutCommandHandler>();
        services.AddScoped<UpdateWorkoutCommandHandler>();
        services.AddScoped<DeleteWorkoutCommandHandler>();
        services.AddScoped<MarkSetAsCompletedCommandHandler>();
        services.AddScoped<MarkSetAsUncompletedCommandHandler>();
        
        return services;
    }
}
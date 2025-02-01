using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WorkoutService.Application.Commands.CreateWorkout;
using WorkoutService.Application.Commands.DeleteWorkout;
using WorkoutService.Application.Commands.MarkSetAsCompleted;
using WorkoutService.Application.Commands.MarkSetAsUncompleted;
using WorkoutService.Application.Commands.UpdateWorkout;
using WorkoutService.Persistence;

namespace WorkoutService.Application.Tests;

public class TestStartup
{
    public static ServiceProvider Initialize(string connectionString)
    {
        var publishEndpointMock = new Mock<IPublishEndpoint>();
        publishEndpointMock
            .Setup(x => x.Publish(It.IsAny<object>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var serviceCollection = new ServiceCollection()
            .AddDbContext<WorkoutDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            })
            .AddScoped<CreateWorkoutCommandHandler>()
            .AddScoped<UpdateWorkoutCommandHandler>()
            .AddScoped<DeleteWorkoutCommandHandler>()
            .AddScoped<MarkSetAsCompletedCommandHandler>()
            .AddScoped<MarkSetAsUncompletedCommandHandler>()
            .AddSingleton(publishEndpointMock.Object);

        return serviceCollection.BuildServiceProvider();
    }
}
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shared.Application.Extensions;
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

        var services = new ServiceCollection();
        
        services.AddLogging();
        
        services.AddDbContext<WorkoutDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
        
        services.AddSingleton(publishEndpointMock.Object);
        
        services.AddCommandHandlers(typeof(DependencyInjection).Assembly);
        services.AddQueryHandlers(typeof(DependencyInjection).Assembly);

        return services.BuildServiceProvider();
    }
}
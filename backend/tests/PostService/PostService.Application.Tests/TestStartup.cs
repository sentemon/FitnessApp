using FluentValidation;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using PostService.Application.DTOs;
using PostService.Persistence;
using Shared.Application.Extensions;

namespace PostService.Application.Tests;

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
        
        services.AddDbContext<PostDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
                
        services
            .AddScoped<IValidator<CreatePostDto>, InlineValidator<CreatePostDto>>()
            .AddScoped<IValidator<CreateCommentDto>, InlineValidator<CreateCommentDto>>();
                
        services.AddCommandHandlers(typeof(DependencyInjection).Assembly);
        services.AddQueryHandlers(typeof(DependencyInjection).Assembly);
            
        services.AddSingleton(publishEndpointMock.Object);

        return services.BuildServiceProvider();
    }
}
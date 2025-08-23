using FluentValidation;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using PostService.Application.DTOs;
using PostService.Persistence;
using Shared.Application.Extensions;
using Shared.DTO.Messages;

namespace PostService.Application.Tests;

public class TestStartup
{
    public static ServiceProvider Initialize(string connectionString)
    {
        var publishEndpointMock = new Mock<IPublishEndpoint>();
        var requestClientMock = new Mock<IRequestClient<PostUploadEventMessage>>();
        
        publishEndpointMock
            .Setup(x => x.Publish(It.IsAny<object>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        requestClientMock
            .Setup(c => c.GetResponse<PostUploadedEventMessage>(
                It.IsAny<PostUploadEventMessage>(),
                It.IsAny<CancellationToken>(),
                default))
            .ReturnsAsync(() =>
            {
                var fileUploaded = new PostUploadedEventMessage(
                    Guid.NewGuid(), 
                    "http://fake.url/file.jpg"
                );

                var responseMock = new Mock<Response<PostUploadedEventMessage>>();
                responseMock.Setup(r => r.Message).Returns(fileUploaded);

                return responseMock.Object;
            });

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
        services.AddSingleton(requestClientMock.Object);

        return services.BuildServiceProvider();
    }
}
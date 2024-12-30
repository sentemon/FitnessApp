using FluentValidation;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PostService.Application.Commands.AddComment;
using PostService.Application.Commands.AddLike;
using PostService.Application.Commands.AddPost;
using PostService.Application.Commands.DeleteComment;
using PostService.Application.Commands.DeleteLike;
using PostService.Application.Commands.UpdatePost;
using PostService.Application.Queries.GetAllComments;
using PostService.Application.Queries.GetAllLikes;
using PostService.Application.Queries.GetAllPosts;
using PostService.Application.Queries.GetPost;
using PostService.Domain.Constants;

namespace PostService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        
        services.AddScoped<AddPostCommandHandler>();
        services.AddScoped<UpdatePostCommandHandler>();
        services.AddScoped<AddCommentCommandHandler>();
        services.AddScoped<DeleteCommentCommandHandler>();
        services.AddScoped<AddLikeCommandHandler>();
        services.AddScoped<DeleteLikeCommandHandler>();
        services.AddScoped<GetPostQueryHandler>();
        services.AddScoped<GetAllPostsQueryHandler>();
        services.AddScoped<GetAllCommentsQueryHandler>();
        services.AddScoped<GetAllLikesQueryHandler>();
        
        var rabbitMqHost = configuration[AppSettingsConstants.RabbitMqHost] ?? throw new ArgumentException("RabbitMQ Host is not configured.");
        var rabbitMqUsername = configuration[AppSettingsConstants.RabbitMqUsername] ?? throw new ArgumentException("RabbitMQ Username is not configured.");
        var rabbitMqPassword = configuration[AppSettingsConstants.RabbitMqPassword] ?? throw new ArgumentException("RabbitMQ Password is not configured.");
        
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(new Uri(rabbitMqHost), host =>
                {
                    host.Username(rabbitMqUsername);
                    host.Password(rabbitMqPassword);
                });
                
                configurator.ConfigureEndpoints(context);
            });
        });
        
        return services;
    }
}
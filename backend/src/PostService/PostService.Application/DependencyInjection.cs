using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PostService.Application.Commands.AddPost;
using PostService.Application.Commands.DeleteComment;

namespace PostService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        
        services.AddScoped<AddPostCommandHandler>();
        services.AddScoped<DeleteCommentCommandHandler>();
        
        return services;
    }
}
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PostService.Application.Commands.AddComment;
using PostService.Application.Commands.AddLike;
using PostService.Application.Commands.AddPost;
using PostService.Application.Commands.DeleteComment;
using PostService.Application.Commands.DeleteLike;
using PostService.Application.Commands.UpdatePost;
using PostService.Application.Queries.GetPost;

namespace PostService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        
        services.AddScoped<AddPostCommandHandler>();
        services.AddScoped<UpdatePostCommandHandler>();
        services.AddScoped<AddCommentCommandHandler>();
        services.AddScoped<DeleteCommentCommandHandler>();
        services.AddScoped<AddLikeCommandHandler>();
        services.AddScoped<DeleteLikeCommandHandler>();
        services.AddScoped<GetPostQueryHandler>();
        
        return services;
    }
}
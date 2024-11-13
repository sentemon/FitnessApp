using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PostService.Application.Commands.AddComment;
using PostService.Application.Commands.AddLike;
using PostService.Application.Commands.AddPost;
using PostService.Application.Commands.DeleteComment;
using PostService.Application.Commands.DeleteLike;
using PostService.Application.Commands.DeletePost;
using PostService.Application.Commands.UpdatePost;
using PostService.Application.DTOs;
using PostService.Persistence;

namespace PostService.Application.Tests;

public class TestStartup
{
    public ServiceProvider Initialize(string connectionString)
    {
        var serviceCollection = new ServiceCollection()
            .AddDbContext<PostDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            })
            .AddScoped<IValidator<CreatePostDto>, InlineValidator<CreatePostDto>>()
            .AddScoped<IValidator<CreateCommentDto>, InlineValidator<CreateCommentDto>>()
            .AddScoped<AddPostCommandHandler>()
            .AddScoped<UpdatePostCommandHandler>()
            .AddScoped<DeletePostCommandHandler>()
            .AddScoped<AddCommentCommandHandler>()
            .AddScoped<DeleteCommentCommandHandler>()
            .AddScoped<AddLikeCommandHandler>()
            .AddScoped<DeleteLikeCommandHandler>();

        return serviceCollection.BuildServiceProvider();
    }
}
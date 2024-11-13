using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PostService.Application.Commands.AddPost;
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
            .AddScoped<AddPostCommandHandler>()
            .AddScoped<UpdatePostCommandHandler>()
            .AddScoped<DeletePostCommandHandler>();

        return serviceCollection.BuildServiceProvider();
    }
}
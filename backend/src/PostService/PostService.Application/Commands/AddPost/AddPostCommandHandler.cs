using FluentValidation;
using PostService.Application.DTOs;
using PostService.Domain.Entities;
using PostService.Domain.Enums;
using PostService.Persistence;
using Shared.Application.Abstractions;
using Shared.Application.Common;
using Shared.Application.Extensions;

namespace PostService.Application.Commands.AddPost;

public class AddPostCommandHandler : ICommandHandler<AddPostCommand, PostDto>
{
    private readonly PostDbContext _context;
    private readonly IValidator<CreatePostDto> _validator;

    public AddPostCommandHandler(PostDbContext context, IValidator<CreatePostDto> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<IResult<PostDto, Error>> HandleAsync(AddPostCommand command)
    {
        var errorMessage = await _validator.ValidateResultAsync(command.CreatePost);

        if (errorMessage != null)
        {
            return Result<PostDto>.Failure(new Error(errorMessage));
        }

        var post = CreatePostForSpecifiedType(command);

        _context.Posts.Add(post);
        await _context.SaveChangesAsync();
        
        var postDto = new PostDto(
            post.Title,
            post.Description,
            post.ContentUrl,
            post.ContentType,
            post.LikeCount,
            post.CommentCount,
            post.CreatedAt);

        return Result<PostDto>.Success(postDto);
    }

    private static Post CreatePostForSpecifiedType(AddPostCommand command)
    {
        return command.CreatePost.ContentType switch
        {
            ContentType.Text => Post.CreateTextPost(command.UserId, command.CreatePost.Title, command.CreatePost.Description),
            
            ContentType.Image => Post.CreateImagePost(command.UserId, command.CreatePost.ContentUrl, command.CreatePost.Title, command.CreatePost.Description),
            
            ContentType.Video => Post.CreateVideoPost(command.UserId, command.CreatePost.ContentUrl, command.CreatePost.Title, command.CreatePost.Description),
            
            _ => throw new InvalidOperationException("Unsupported content type.")
        };
    }
}
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PostService.Application.DTOs;
using PostService.Application.Validators;
using PostService.Domain.Constants;
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

    public AddPostCommandHandler(PostDbContext context)
    {
        _context = context;
        _validator = new CreatePostValidator();
    }

    public async Task<IResult<PostDto, Error>> HandleAsync(AddPostCommand command)
    {
        var errorMessage = await _validator.ValidateResultAsync(command.CreatePost);

        if (errorMessage != null)
        {
            return Result<PostDto>.Failure(new Error(errorMessage));
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == command.UserId);
        
        if (user == null)
        {
            return Result<PostDto>.Failure(new Error(ResponseMessages.UserNotFound));
        }

        var post = CreatePostForSpecifiedType(command);

        _context.Posts.Add(post);
        await _context.SaveChangesAsync();
        
        var postDto = new PostDto(
            post.Id,
            post.Title,
            post.Description,
            post.ContentUrl,
            post.ContentType,
            post.LikeCount,
            post.CommentCount,
            post.CreatedAt,
            user.ImageUrl,
            user.Username);

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
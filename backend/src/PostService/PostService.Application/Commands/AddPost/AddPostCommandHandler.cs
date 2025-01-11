using FluentValidation;
using MassTransit;
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
using Shared.DTO.Messages;

namespace PostService.Application.Commands.AddPost;

public class AddPostCommandHandler : ICommandHandler<AddPostCommand, PostDto>
{
    private readonly PostDbContext _context;
    private readonly IPublishEndpoint _publishEndpoint;
    
    private readonly IValidator<CreatePostDto> _validator;

    public AddPostCommandHandler(PostDbContext context, IPublishEndpoint publishEndpoint)
    {
        _context = context;
        _publishEndpoint = publishEndpoint;
        
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

        var post = CreatePostForSpecifiedType(command, user.Id);

        _context.Posts.Add(post);
        await _context.SaveChangesAsync();

        await _publishEndpoint.Publish(new PostUploadEventMessage(
            command.CreatePost.File.OpenReadStream(),
            command.CreatePost.FileContentType,
            post.Id,
            post.UserId
        ));
        
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
            user.Username
        );

        return Result<PostDto>.Success(postDto);
    }

    private static Post CreatePostForSpecifiedType(AddPostCommand command, string userId)
    {
        return command.CreatePost.ContentType switch
        {
            ContentType.Text => Post.CreateTextPost(userId, command.CreatePost.Title, command.CreatePost.Description),
            
            ContentType.Image => Post.CreateImagePost(userId, command.CreatePost.Title, command.CreatePost.Description),
            
            ContentType.Video => Post.CreateVideoPost(userId, command.CreatePost.Title, command.CreatePost.Description),
            
            _ => throw new InvalidOperationException("Unsupported content type.")
        };
    }
}
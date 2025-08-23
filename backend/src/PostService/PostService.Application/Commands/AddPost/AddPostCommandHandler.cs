using FluentValidation;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
    private readonly IRequestClient<PostUploadEventMessage> _client;
    private readonly ILogger<AddPostCommandHandler> _logger;
    
    private readonly IValidator<CreatePostDto> _validator;

    public AddPostCommandHandler(PostDbContext context, IRequestClient<PostUploadEventMessage> client, ILogger<AddPostCommandHandler> logger)
    {
        _context = context;
        _client = client;
        _logger = logger;

        _validator = new CreatePostValidator();
    }

    public async Task<IResult<PostDto, Error>> HandleAsync(AddPostCommand command)
    {
        var errorMessage = await _validator.ValidateResultAsync(command.CreatePost);

        if (errorMessage is not null)
        {
            _logger.LogWarning("Validation failed for AddPostCommand: {ErrorMessage}", errorMessage);
            return Result<PostDto>.Failure(new Error(errorMessage));
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == command.UserId);
        
        if (user == null)
        {
            _logger.LogWarning("Attempted to add a post with a user that does not exist: UserId: {UserId}", command.UserId);
            return Result<PostDto>.Failure(new Error(ResponseMessages.UserNotFound));
        }

        try
        {
            var post = CreatePostForSpecifiedType(command, user.Id);
            
            _context.Posts.Add(post);

            if (command.CreatePost.ContentType == ContentType.Text && command.CreatePost.File != null ||
                command.CreatePost.ContentType != ContentType.Text && command.CreatePost.File == null)
            {
                _logger.LogWarning("Invalid file state for post creation: ContentType: {ContentType}, File: {FileState}", 
                    command.CreatePost.ContentType, command.CreatePost.File == null ? "null" : "not null");
                return Result<PostDto>.Failure(new Error(ResponseMessages.InvalidFileState));
            }

            var fileContentType = FileExtensions.GetContentType(command.CreatePost.File);

            var response = await _client.GetResponse<PostUploadedEventMessage>(new PostUploadEventMessage(
                FileExtensions.ReadFully(command.CreatePost.File?.OpenReadStream()),
                fileContentType,
                post.Id,
                post.UserId
            ));

            _logger.LogInformation("Post upload event processed successfully for PostId: {PostId}, UserId: {UserId}", response.Message.PostId, post.UserId);
            
            post.SetContentUrl(response.Message.ContentUrl);
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
                user.Username
            );

            return Result<PostDto>.Success(postDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while adding a post: {Message}", ex.Message);
            return Result<PostDto>.Failure(new Error(ex.Message));
        }
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
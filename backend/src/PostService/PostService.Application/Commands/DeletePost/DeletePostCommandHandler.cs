using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PostService.Domain.Constants;
using PostService.Persistence;
using Shared.Application.Abstractions;
using Shared.Application.Common;
using Shared.DTO.Messages;

namespace PostService.Application.Commands.DeletePost;

public class DeletePostCommandHandler : ICommandHandler<DeletePostCommand, string>
{
    private readonly PostDbContext _context;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<DeletePostCommandHandler> _logger;

    public DeletePostCommandHandler(PostDbContext context, IPublishEndpoint publishEndpoint, ILogger<DeletePostCommandHandler> logger)
    {
        _context = context;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<IResult<string, Error>> HandleAsync(DeletePostCommand command)
    {
        var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == command.Id);

        if (post == null)
        {
            _logger.LogWarning("Attempted to delete a post that does not exist: PostId: {PostId}", command.Id);
            return Result<string>.Failure(new Error(ResponseMessages.PostNotFound));
        }

        if (post.UserId != command.UserId)
        {
            _logger.LogWarning("User {UserId} attempted to delete a post they do not own: PostId: {PostId}", command.UserId, command.Id);
            return Result<string>.Failure(new Error(ResponseMessages.YouDoNotHavePermissionToDeleteThisPost));
        }
        
        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();

        await _publishEndpoint.Publish(new PostDeletedEventMessage(
            post.Id
        ));

        return Result<string>.Success(ResponseMessages.YouSuccessfullyDeletedPost);
    }
}
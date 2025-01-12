using MassTransit;
using Microsoft.EntityFrameworkCore;
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

    public DeletePostCommandHandler(PostDbContext context, IPublishEndpoint publishEndpoint)
    {
        _context = context;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<IResult<string, Error>> HandleAsync(DeletePostCommand command)
    {
        var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == command.Id);

        if (post == null)
        {
            return Result<string>.Failure(new Error(ResponseMessages.PostNotFound));
        }

        if (post.UserId != command.UserId)
        {
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
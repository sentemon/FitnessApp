using FileService.Application.Commands.DeletePost;
using MassTransit;
using Shared.DTO.Messages;

namespace FileService.Application.Consumers;

public class PostDeletedEventConsumer : IConsumer<PostDeletedEventMessage>
{
    private readonly DeletePostCommandHandler _deletePostCommandHandler;

    public PostDeletedEventConsumer(DeletePostCommandHandler deletePostCommandHandler)
    {
        _deletePostCommandHandler = deletePostCommandHandler;
    }

    public async Task Consume(ConsumeContext<PostDeletedEventMessage> context)
    {
        var @event = context.Message;

        var command = new DeletePostCommand(@event.PostId);

        var result = await _deletePostCommandHandler.HandleAsync(command);

        if (!result.IsSuccess)
        {
            // log
        }
    }
}
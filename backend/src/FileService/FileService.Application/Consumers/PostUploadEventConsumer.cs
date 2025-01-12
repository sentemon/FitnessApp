using FileService.Application.Commands.UploadPost;
using FileService.Application.DTOs;
using MassTransit;
using Shared.DTO.Messages;

namespace FileService.Application.Consumers;

public class PostUploadEventConsumer : IConsumer<PostUploadEventMessage>
{
    private readonly UploadPostCommandHandler _uploadPostCommandHandler;

    public PostUploadEventConsumer(UploadPostCommandHandler uploadPostCommandHandler)
    {
        _uploadPostCommandHandler = uploadPostCommandHandler;
    }

    public async Task Consume(ConsumeContext<PostUploadEventMessage> context)
    {
        var @event = context.Message;

        var fileStream = new MemoryStream(@event.FileData);

        var command = new UploadPostCommand(new UploadPostFileDto(
                fileStream,
                @event.ContentType,
                @event.PostId
            ),
            @event.UserId
        );

        var result = await _uploadPostCommandHandler.HandleAsync(command);

        if (!result.IsSuccess)
        {
            // log
        }
    }
}
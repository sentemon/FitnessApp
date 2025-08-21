using FileService.Application.Commands.UploadPost;
using FileService.Application.DTOs;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.DTO.Messages;

namespace FileService.Application.Consumers;

public class PostUploadEventConsumer : IConsumer<PostUploadEventMessage>
{
    private readonly UploadPostCommandHandler _uploadPostCommandHandler;
    private readonly ILogger<PostUploadEventConsumer> _logger;

    public PostUploadEventConsumer(UploadPostCommandHandler uploadPostCommandHandler, ILogger<PostUploadEventConsumer> logger)
    {
        _uploadPostCommandHandler = uploadPostCommandHandler;
        _logger = logger;
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
            _logger.LogError("Failed to upload post file for PostId: {PostId}, UserId: {UserId}. Error: {ErrorMessage}", @event.PostId, @event.UserId, result.Error.Message);
            throw new Exception($"Failed to upload post file: {result.Error.Message}");
        }
    }
}
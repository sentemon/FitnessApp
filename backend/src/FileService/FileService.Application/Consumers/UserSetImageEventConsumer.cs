using FileService.Application.Commands.SetUserImage;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.DTO.Messages;

namespace FileService.Application.Consumers;

public class UserSetImageEventConsumer : IConsumer<UserSetImageEventMessage>
{
    private readonly SetUserImageCommandHandler _setUserImageCommandHandler;
    private readonly ILogger<UserSetImageEventConsumer> _logger;

    public UserSetImageEventConsumer(SetUserImageCommandHandler setUserImageCommandHandler, ILogger<UserSetImageEventConsumer> logger)
    {
        _setUserImageCommandHandler = setUserImageCommandHandler;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<UserSetImageEventMessage> context)
    {
        var @event = context.Message;
        
        var fileStream = new MemoryStream(@event.FileData);
        
        var command = new SetUserImageCommand(
            fileStream,
            @event.ContentType,
            @event.UserId
        );
        
        var result = await _setUserImageCommandHandler.HandleAsync(command);
        
        if (!result.IsSuccess)
        {
            _logger.LogError("Failed to set user image for UserId: {UserId}. Error: {ErrorMessage}", @event.UserId, result.Error.Message);
            throw new Exception($"Failed to set user image: {result.Error.Message}");
        }
    }
}
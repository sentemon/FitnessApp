using AuthService.Application.Commands.SetImageUrl;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.DTO.Messages;

namespace AuthService.Application.Consumers;

public class UserImageSetEventConsumer : IConsumer<UserImageSetEventMessage>
{
    private readonly SetImageUrlCommandHandler _setImageUrlCommandHandler;
    private readonly ILogger<UserImageSetEventConsumer> _logger;

    public UserImageSetEventConsumer(SetImageUrlCommandHandler setImageUrlCommandHandler, ILogger<UserImageSetEventConsumer> logger)
    {
        _setImageUrlCommandHandler = setImageUrlCommandHandler;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<UserImageSetEventMessage> context)
    {
        var @event = context.Message;
        
        var command = new SetImageUrlCommand(@event.ImageUrl, @event.UserId);
        var result = await _setImageUrlCommandHandler.HandleAsync(command);
        
        if (!result.IsSuccess)
        {
            _logger.LogError("Failed to set user image for UserId: {UserId}. Error: {ErrorMessage}", @event.UserId, result.Error.Message);
            throw new Exception($"Failed to set user image: {result.Error.Message}");
        }
    }
}
namespace Shared.DTO.Messages;

public record PostUploadedEventMessage(
    Guid PostId,
    string ContentUrl
);
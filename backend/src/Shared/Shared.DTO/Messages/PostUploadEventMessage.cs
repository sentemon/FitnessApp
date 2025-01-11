namespace Shared.DTO.Messages;

public record PostUploadEventMessage(
    Stream FileStream,
    string ContentType,
    Guid PostId,
    string UserId
);
namespace Shared.DTO.Messages;

public record PostUploadEventMessage(
    byte[] FileData,
    string? ContentType,
    Guid PostId,
    string UserId
);
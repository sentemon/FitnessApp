namespace Shared.DTO.Messages;

public record UserSetImageEventMessage(
    byte[] FileData,
    string? ContentType,
    string UserId
);
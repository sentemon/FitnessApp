namespace Shared.DTO.Messages;

public record PostDeletedEventMessage(
    Guid PostId
);
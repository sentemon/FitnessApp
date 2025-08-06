namespace Shared.DTO.Messages;

public record ActivityStatusUpdatedEventMessage(
    string UserId,
    DateTime LastSeenAt
);
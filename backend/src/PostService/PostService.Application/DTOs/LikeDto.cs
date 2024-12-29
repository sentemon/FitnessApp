namespace PostService.Application.DTOs;

public record LikeDto(
    Guid Id,
    Guid PostId,
    string UserId,
    DateTime CreatedAt
);
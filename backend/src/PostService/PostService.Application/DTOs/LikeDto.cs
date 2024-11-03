namespace PostService.Application.DTOs;

public record LikeDto(
    Guid PostId,
    Guid UserId,
    DateTime CreatedAt);
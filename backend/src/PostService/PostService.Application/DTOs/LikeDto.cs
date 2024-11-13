namespace PostService.Application.DTOs;

public record LikeDto(
    Guid Id,
    Guid PostId,
    Guid UserId,
    DateTime CreatedAt);
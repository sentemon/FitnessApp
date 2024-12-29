namespace PostService.Application.DTOs;

public record CommentDto(
    Guid Id,
    Guid PostId,
    string UserId,
    string Username,
    string Content,
    DateTime CreatedAt
);
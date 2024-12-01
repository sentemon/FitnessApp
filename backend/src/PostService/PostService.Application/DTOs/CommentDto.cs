namespace PostService.Application.DTOs;

public record CommentDto(
    Guid Id,
    Guid PostId,
    Guid UserId,
    string Username,
    string Content,
    DateTime CreatedAt);
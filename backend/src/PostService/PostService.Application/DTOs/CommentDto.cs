namespace PostService.Application.DTOs;

public record CommentDto(
    Guid PostId,
    Guid UserId,
    string Content,
    DateTime CreatedAt);
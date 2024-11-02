using PostService.Domain.Enums;

namespace PostService.Application.DTOs;

public record PostDto(
    string Title,
    string Description,
    string ContentUrl,
    ContentType ContentType,
    int LikeCount,
    int CommentCount,
    DateTime CreatedAt);
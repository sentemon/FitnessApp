using PostService.Domain.Enums;

namespace PostService.Application.DTOs;

public record PostDto(
    Guid Id,
    string Title,
    string Description,
    string ContentUrl,
    ContentType ContentType,
    uint LikeCount,
    uint CommentCount,
    DateTime CreatedAt,
    string UserImageUrl,
    string Username);
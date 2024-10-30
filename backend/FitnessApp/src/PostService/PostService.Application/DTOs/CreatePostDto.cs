using PostService.Domain.Enums;

namespace PostService.Application.DTOs;

public record CreatePostDto(
    Guid UserId,
    string Title,
    string Description,
    string ContentUrl,
    ContentType ContentType);
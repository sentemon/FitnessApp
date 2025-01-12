using HotChocolate.Types;
using PostService.Domain.Enums;

namespace PostService.Application.DTOs;

public record CreatePostDto(
    string Title,
    string Description,
    IFile? File,
    ContentType ContentType
);
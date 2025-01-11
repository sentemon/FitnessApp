using Microsoft.AspNetCore.Http;
using PostService.Domain.Enums;

namespace PostService.Application.DTOs;

public record CreatePostDto(
    string Title,
    string Description,
    IFormFile File,
    string FileContentType,
    ContentType ContentType
);
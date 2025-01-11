namespace FileService.Application.DTOs;

public record UploadPostFileDto(
    Stream? FileStream,
    string? ContentType,
    Guid PostId
);
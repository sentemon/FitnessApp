
using FileService.Application.DTOs;
using Shared.Application.Abstractions;

namespace FileService.Application.Commands.UploadPost;

public record UploadPostCommand(UploadPostFileDto UploadPostFileDto, string? UserId) : ICommand;
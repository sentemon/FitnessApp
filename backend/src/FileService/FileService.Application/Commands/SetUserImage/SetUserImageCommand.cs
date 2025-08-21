using Shared.Application.Abstractions;

namespace FileService.Application.Commands.SetUserImage;

public record SetUserImageCommand(Stream? FileStream, string? ContentType, string? UserId) : ICommand;
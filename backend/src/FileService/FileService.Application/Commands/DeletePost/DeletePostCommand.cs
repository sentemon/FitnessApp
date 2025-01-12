using Shared.Application.Abstractions;

namespace FileService.Application.Commands.DeletePost;

public record DeletePostCommand(Guid PostId) : ICommand;
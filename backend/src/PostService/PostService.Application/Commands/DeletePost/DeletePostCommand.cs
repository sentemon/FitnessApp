using Shared.Application.Abstractions;

namespace PostService.Application.Commands.DeletePost;

public record DeletePostCommand(Guid Id, string UserId) : ICommand;
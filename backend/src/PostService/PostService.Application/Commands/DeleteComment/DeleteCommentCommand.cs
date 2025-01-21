using Shared.Application.Abstractions;

namespace PostService.Application.Commands.DeleteComment;

public record DeleteCommentCommand(Guid Id, string? UserId) : ICommand;
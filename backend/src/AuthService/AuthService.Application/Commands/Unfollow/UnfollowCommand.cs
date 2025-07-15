using Shared.Application.Abstractions;

namespace AuthService.Application.Commands.Unfollow;

public record UnfollowCommand(string TargetUserId, string? UserId) : ICommand;
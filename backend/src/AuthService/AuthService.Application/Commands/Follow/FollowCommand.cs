using Shared.Application.Abstractions;

namespace AuthService.Application.Commands.Follow;

public record FollowCommand(string TargetUserId, string? UserId) : ICommand;
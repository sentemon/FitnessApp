using Shared.Application.Abstractions;

namespace AuthService.Application.Commands.UpdateActivityStatus;

public record UpdateActivityStatusCommand(string? UserId) : ICommand;
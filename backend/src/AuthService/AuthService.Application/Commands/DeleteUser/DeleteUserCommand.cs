using Shared.Application.Abstractions;

namespace AuthService.Application.Commands.DeleteUser;

public record DeleteUserCommand(string? UserId) : ICommand;
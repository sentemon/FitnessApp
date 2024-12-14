using Shared.Application.Abstractions;

namespace AuthService.Application.Commands.Logout;

public record LogoutCommand(string RefreshToken) : ICommand;
using Shared.Application.Abstractions;

namespace AuthService.Application.Commands.VerifyEmail;

public record VerifyEmailCommand(string UserId) : ICommand;
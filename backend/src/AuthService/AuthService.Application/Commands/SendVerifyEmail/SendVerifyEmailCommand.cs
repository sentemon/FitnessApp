using Shared.Application.Abstractions;

namespace AuthService.Application.Commands.SendVerifyEmail;

public record SendVerifyEmailCommand(string UserId) : ICommand;
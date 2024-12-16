using Shared.Application.Abstractions;

namespace AuthService.Application.Commands.ResetPassword;

public record ResetPasswordCommand(string UserId, string NewPassword) : ICommand;
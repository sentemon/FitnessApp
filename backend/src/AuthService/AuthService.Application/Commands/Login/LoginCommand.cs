using AuthService.Application.DTOs;
using Shared.Application.Abstractions;

namespace AuthService.Application.Commands.Login;

public record LoginCommand(LoginDto LoginDto) : ICommand;
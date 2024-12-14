using AuthService.Application.DTOs;
using Shared.Application.Abstractions;

namespace AuthService.Application.Commands.Register;

public record RegisterCommand(RegisterDto RegisterDto) : ICommand;
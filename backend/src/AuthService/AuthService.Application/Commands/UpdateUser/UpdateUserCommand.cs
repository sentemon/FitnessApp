using AuthService.Application.DTOs;
using Shared.Application.Abstractions;

namespace AuthService.Application.Commands.UpdateUser;

public record UpdateUserCommand(UpdateUserDto UpdateUserDto, string? UserId) : ICommand;
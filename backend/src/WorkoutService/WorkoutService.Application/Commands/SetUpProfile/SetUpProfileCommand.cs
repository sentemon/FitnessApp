using Shared.Application.Abstractions;
using WorkoutService.Application.DTOs;

namespace WorkoutService.Application.Commands.SetUpProfile;

public record SetUpProfileCommand(SetUpProfileDto SetUpProfileDto, string? UserId) : ICommand;
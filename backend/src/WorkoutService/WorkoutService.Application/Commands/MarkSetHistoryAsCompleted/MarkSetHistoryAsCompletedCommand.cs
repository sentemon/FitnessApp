using Shared.Application.Abstractions;

namespace WorkoutService.Application.Commands.MarkSetHistoryAsCompleted;

public record MarkSetHistoryAsCompletedCommand(Guid Id) : ICommand;
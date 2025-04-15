using Shared.Application.Abstractions;

namespace ChatService.Application.Commands.SendMessage;

public record SendMessageCommand(Guid ChatId, string Content, string? UserId) : ICommand;
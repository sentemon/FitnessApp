using Shared.Application.Abstractions;

namespace ChatService.Application.Commands.SendMessage;

public record SendMessageCommand(string ReceiverId, string Content, string? UserId) : ICommand;
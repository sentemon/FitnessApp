using Shared.Application.Abstractions;

namespace ChatService.Application.Commands.CreateChat;

public record CreateChatCommand(string? UserId1, string? UserId2) : ICommand;
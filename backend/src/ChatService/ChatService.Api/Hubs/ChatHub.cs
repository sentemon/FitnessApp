using System.Security.Claims;
using ChatService.Application.Commands.CreateChat;
using ChatService.Application.Commands.SendMessage;
using ChatService.Domain.Constants;
using Microsoft.AspNetCore.SignalR;

namespace ChatService.Api.Hubs;

public class ChatHub : Hub
{
    private readonly SendMessageCommandHandler _sendMessageCommandHandler;
    private readonly CreateChatCommandHandler _createChatCommandHandler;

    public ChatHub(SendMessageCommandHandler sendMessageCommandHandler, CreateChatCommandHandler createChatCommandHandler)
    {
        _sendMessageCommandHandler = sendMessageCommandHandler;
        _createChatCommandHandler = createChatCommandHandler;
    }

    public async Task SendMessage(string receiverId, string message)
    {
        var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var command = new SendMessageCommand(receiverId, message, userId);
        var result = await _sendMessageCommandHandler.HandleAsync(command);

        if (!result.IsSuccess)
        {
            throw new HubException(result.Error.Message);
        }

        var chatId = result.Response.ChatId.ToString();

        await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
        
        await Clients.Group(chatId).SendAsync("ReceiveMessage", result.Response);
    }
    
    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        var chatIdParam = Context.GetHttpContext()?.Request.Query["chatId"].ToString();
        var receiverIdParam = Context.GetHttpContext()?.Request.Query["receiverId"].ToString();

        if (string.IsNullOrWhiteSpace(chatIdParam) && string.IsNullOrWhiteSpace(receiverIdParam))
        {
            throw new HubException(ResponseMessages.ChatIdCannotBeEmpty);
        }

        var chatId = await ResolveChatIdAsync(chatIdParam, receiverIdParam, userId);

        await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
        await base.OnConnectedAsync();
    }

    private async Task<string> ResolveChatIdAsync(string? chatIdParam, string? receiverIdParam, string? userId)
    {
        if (!string.IsNullOrWhiteSpace(chatIdParam))
        {
            return chatIdParam;
        }

        var command = new CreateChatCommand(userId, receiverIdParam);
        var result = await _createChatCommandHandler.HandleAsync(command);
        
        if (!result.IsSuccess)
        {
            throw new HubException(result.Error.Message);
        }

        return result.Response.Id.ToString();
    }
}
using System.Security.Claims;
using ChatService.Application.Commands.SendMessage;
using Microsoft.AspNetCore.SignalR;

namespace ChatService.Api.Hubs;

public class ChatHub : Hub
{
    private readonly SendMessageCommandHandler _sendMessageCommandHandler;

    public ChatHub(SendMessageCommandHandler sendMessageCommandHandler)
    {
        _sendMessageCommandHandler = sendMessageCommandHandler;
    }

    public async Task SendMessage(string chatId, string message)
    {
        var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var command = new SendMessageCommand(Guid.Parse(chatId), message, userId);
        var result = await _sendMessageCommandHandler.HandleAsync(command);

        if (!result.IsSuccess)
        {
            throw new HubException(result.Error.Message);
        }
        
        await Clients.Group(chatId).SendAsync("ReceiveMessage", result.Response);
    }
    
    public override async Task OnConnectedAsync()
    {
        var chatId = Context.GetHttpContext()?.Request.Query["chatId"].ToString();

        if (string.IsNullOrEmpty(chatId))
        {
            throw new HubException("Chat Id cannot be empty.");
        }
        
        await Groups.AddToGroupAsync(Context.ConnectionId, chatId);

        await base.OnConnectedAsync();
    }
}
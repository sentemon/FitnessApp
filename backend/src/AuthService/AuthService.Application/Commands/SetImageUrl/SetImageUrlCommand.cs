using Shared.Application.Abstractions;

namespace AuthService.Application.Commands.SetImageUrl;

public record SetImageUrlCommand(string ImageUrl, string? UserId) : ICommand;
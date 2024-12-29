using Shared.Application.Abstractions;

namespace PostService.Application.Commands.AddLike;

public record AddLikeCommand(Guid PostId, string UserId) : ICommand;
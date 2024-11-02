using PostService.Application.DTOs;
using Shared.Application.Abstractions;

namespace PostService.Application.Commands.UpdatePost;

public record UpdatePostCommand(UpdatePostDto UpdatePost, Guid UserId) : ICommand;
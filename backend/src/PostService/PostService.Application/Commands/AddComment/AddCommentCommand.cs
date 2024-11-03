using PostService.Application.DTOs;
using Shared.Application.Abstractions;

namespace PostService.Application.Commands.AddComment;

public record AddCommentCommand(CreateCommentDto CreateComment, Guid UserId) : ICommand;
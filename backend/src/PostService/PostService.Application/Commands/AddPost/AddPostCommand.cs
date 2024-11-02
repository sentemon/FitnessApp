using PostService.Application.DTOs;
using Shared.Application.Abstractions;

namespace PostService.Application.Commands.AddPost;

public record AddPostCommand(CreatePostDto CreatePost, Guid UserId) : ICommand;
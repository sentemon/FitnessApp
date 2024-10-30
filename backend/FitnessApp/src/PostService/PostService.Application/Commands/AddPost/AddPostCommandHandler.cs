using PostService.Application.DTOs;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace PostService.Application.Commands.AddPost;

public class AddPostCommandHandler : ICommandHandler<AddPostCommand, PostDto>
{
    public Task<IResult<PostDto, Error>> HandleAsync(AddPostCommand command)
    {
        throw new NotImplementedException();
    }
}
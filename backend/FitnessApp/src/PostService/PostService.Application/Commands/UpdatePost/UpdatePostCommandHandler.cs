using PostService.Application.DTOs;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace PostService.Application.Commands.UpdatePost;

public class UpdatePostCommandHandler : ICommandHandler<UpdatePostCommand, PostDto>
{
    public async Task<IResult<PostDto, Error>> HandleAsync(UpdatePostCommand command)
    {
        throw new NotImplementedException();
    }
}
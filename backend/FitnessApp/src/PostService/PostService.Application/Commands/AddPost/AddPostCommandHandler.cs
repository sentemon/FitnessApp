using Shared.Application.Abstractions;
using Shared.Application.Errors;

namespace PostService.Application.Commands.AddPost;

public class AddPostCommandHandler : ICommandHandler<AddPostCommand, bool>
{
    public Task<IResult<bool, Error>> HandleAsync(AddPostCommand command)
    {
        throw new NotImplementedException();
    }
}
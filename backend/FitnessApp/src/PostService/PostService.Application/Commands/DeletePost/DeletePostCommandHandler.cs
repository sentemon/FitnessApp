using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace PostService.Application.Commands.DeletePost;

public class DeletePostCommandHandler : ICommandHandler<DeletePostCommand, Unit>
{
    public async Task<IResult<Unit, Error>> HandleAsync(DeletePostCommand command)
    {
        throw new NotImplementedException();
    }
}
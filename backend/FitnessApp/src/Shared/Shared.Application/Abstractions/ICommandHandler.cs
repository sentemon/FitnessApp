using Shared.Application.Errors;

namespace Shared.Application.Abstractions;

public interface ICommandHandler<in TCommand, TResponse> where TCommand : ICommand
{
    Task<IResult<TResponse, Error>> HandleAsync(TCommand command);
}
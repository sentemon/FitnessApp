using Shared.Application.Common;

namespace Shared.Application.Abstractions;

public interface IQueryHandler<in TQuery, TResponse> where TQuery : IQuery
{
    Task<IResult<TResponse, Error>> HandleAsync(TQuery query);
}
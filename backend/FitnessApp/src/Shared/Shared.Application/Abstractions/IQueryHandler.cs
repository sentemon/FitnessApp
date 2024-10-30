namespace Shared.Application.Abstractions;

public interface IQueryHandler<in TQuery, TResponse> where TQuery : IQuery
{
    Task<TResponse> HandleAsync(TQuery query);
}
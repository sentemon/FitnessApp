using Shared.Application.Abstractions;

namespace PostService.Application.Queries.GetPost;

public record GetPostQuery(Guid Id) : IQuery;
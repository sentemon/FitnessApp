using Shared.Application.Abstractions;

namespace PostService.Application.Queries.GetAllPosts;

public record GetAllPostsQuery(int First, string AfterCursor) : IQuery;
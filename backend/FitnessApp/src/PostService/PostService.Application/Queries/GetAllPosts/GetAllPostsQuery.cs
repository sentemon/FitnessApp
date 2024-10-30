using PostService.Domain.Entities;
using Shared.Application.Abstractions;

namespace PostService.Application.Queries.GetAllPosts;

public record GetAllPostsQuery() : IQuery<IQueryable<Post>>;
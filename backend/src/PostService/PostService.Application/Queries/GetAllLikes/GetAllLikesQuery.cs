using Shared.Application.Abstractions;

namespace PostService.Application.Queries.GetAllLikes;

public record GetAllLikesQuery(Guid PostId, int First = 20) : IQuery;
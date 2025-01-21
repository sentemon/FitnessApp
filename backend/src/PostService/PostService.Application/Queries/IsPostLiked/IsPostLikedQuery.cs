using Shared.Application.Abstractions;

namespace PostService.Application.Queries.IsPostLiked;

public record IsPostLikedQuery(Guid PostId, string? UserId) : IQuery;
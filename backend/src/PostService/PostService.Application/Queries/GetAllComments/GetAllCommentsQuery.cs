using Shared.Application.Abstractions;

namespace PostService.Application.Queries.GetAllComments;

public record GetAllCommentsQuery(Guid PostId, int First = 20) : IQuery;
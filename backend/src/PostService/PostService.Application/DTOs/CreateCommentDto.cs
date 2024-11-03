namespace PostService.Application.DTOs;

public record CreateCommentDto(Guid PostId, string Content);
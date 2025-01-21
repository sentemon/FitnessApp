namespace PostService.Application.DTOs;

public record UpdatePostDto(
    string Id, 
    string Title,
    string Description
);
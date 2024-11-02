namespace PostService.Application.DTOs;

public record UpdatePostDto(
    Guid Id, 
    string Title,
    string Description);
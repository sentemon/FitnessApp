using HotChocolate.Types;

namespace AuthService.Application.DTOs;

public record UpdateUserDto(
    IFile? Image,
    string FirstName,
    string LastName,
    string Username,
    string Email,
    string Bio
);
// Dtos/CreateEtudiantDto.cs
namespace AuthService.Dtos;

public record CreateEtudiantDto(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string FiliereId
);
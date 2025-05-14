// Dtos/UpdateEnseignantDto.cs
namespace AuthService.Dtos;

public record UpdateEnseignantDto(
    string FirstName,
    string LastName,
    string Grade,
    string Specialite
);
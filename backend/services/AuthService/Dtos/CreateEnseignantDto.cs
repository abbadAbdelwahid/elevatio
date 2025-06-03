// Dtos/CreateEnseignantDto.cs
namespace AuthService.Dtos;

public record CreateEnseignantDto(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string Grade,
    string Specialite,
    DateTime? DateEmbauche);
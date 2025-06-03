// Dtos/CreateExternalEvaluatorDto.cs
namespace AuthService.Dtos;

public record CreateExternalEvaluatorDto(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string Organisation,
    string Domaine
);
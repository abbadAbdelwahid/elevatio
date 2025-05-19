// Dtos/UpdateExternalEvaluatorDto.cs
namespace AuthService.Dtos;

public record UpdateExternalEvaluatorDto(
    string FirstName,
    string LastName,
    string Organisation,
    string Domaine
);
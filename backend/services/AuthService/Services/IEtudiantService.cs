// Services/IEtudiantService.cs
using AuthService.Dtos;
using AuthService.Models;

namespace AuthService.Services;

public interface IEtudiantService
{
    Task<(bool Ok, IEnumerable<string> Errors, Etudiant? Data)> CreateAsync(CreateEtudiantDto dto);
    Task<Etudiant?> GetByIdAsync(string id);
}
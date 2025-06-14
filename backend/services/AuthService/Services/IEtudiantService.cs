// Services/IEtudiantService.cs
using AuthService.Dtos;
using AuthService.Models;

namespace AuthService.Services;

public interface IEtudiantService
{
    Task<(bool Ok, IEnumerable<string> Errors, Etudiant? Data)> CreateAsync(CreateEtudiantDto dto);
    Task<Etudiant?> GetByIdAsync(string id);
    public Task<string> UploadProfileImage(IFormFile file);

    Task<string?> GetFullNameByIdAsync(string id);
    
    Task<IEnumerable<Etudiant>> GetAllAsync();
    public Task<IEnumerable<Etudiant>> GetByFiliereIdAsync(int filiereId);

}
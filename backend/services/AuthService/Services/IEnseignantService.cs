// Services/IEnseignantService.cs
using AuthService.Dtos;
using AuthService.Models;

namespace AuthService.Services;

public interface IEnseignantService
{
    Task<(bool Ok, IEnumerable<string> Errors, Enseignant? Data)> CreateAsync(CreateEnseignantDto dto);
    Task<Enseignant?> GetByIdAsync(string id);
    Task<bool> UpdateAsync(string id, UpdateEnseignantDto dto, string currentUserId);
    Task<bool> DeleteAsync(string id);
    public Task<string> UploadProfileImage(IFormFile file);
    Task<string?> GetFullNameByIdAsync(string id);



}
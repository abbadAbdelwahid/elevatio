// Services/IExternalEvaluatorService.cs
using AuthService.Dtos;
using AuthService.Models;

namespace AuthService.Services;

public interface IExternalEvaluatorService
{
    Task<(bool Ok, IEnumerable<string> Errors, ExternalEvaluator? Data)> CreateAsync(CreateExternalEvaluatorDto dto);
    Task<ExternalEvaluator?> GetByIdAsync(string id);
    Task<IEnumerable<ExternalEvaluator>> GetAllAsync(string? domaine = null);
    Task<bool> UpdateAsync(string id, UpdateExternalEvaluatorDto dto);
    Task<bool> DeleteAsync(string id);

}
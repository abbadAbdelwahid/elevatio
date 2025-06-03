namespace EvaluationService.Services;

public interface IExternalValidationService
{
    // Vérifie l'existence d'un seul utilisateur
    Task<bool> UserExistsAsync(string userId);

    // Vérifie l'existence d'un seul module
    Task<bool> ModuleExistsAsync(int moduleId);

    // Vérifie l'existence d'une seule filière
    Task<bool> FiliereExistsAsync(int filiereId);

    // Vérifie l'existence de plusieurs utilisateurs en batch
    Task<Dictionary<string, bool>> UsersExistAsync(List<string?> userIds);

    // Vérifie l'existence de plusieurs modules en batch
    Task<Dictionary<int, bool>> ModulesExistAsync(List<int?> moduleIds);

    // Vérifie l'existence de plusieurs filières en batch
    Task<Dictionary<int, bool>> FilieresExistAsync(List<int?> filiereIds);
}


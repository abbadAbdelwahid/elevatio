using AnalyticsService.ExternalClients.DTO ;
namespace AnalyticsService.ExternalClients.ClientInterfaces;

public interface IEvaluationClient
{
    /// <summary>
    /// Récupère toutes les évaluations associées à un module.
    /// </summary>
    Task<IEnumerable<EvaluationDto>> GetByModuleAsync(int moduleId);

    /// <summary>
    /// Récupère toutes les évaluations associées à une filière.
    /// </summary>
    Task<IEnumerable<EvaluationDto>> GetByFiliereAsync(int filiereId);

    /// <summary>
    /// Récupère toutes les évaluations associées à un enseignant.
    /// </summary>
    Task<IEnumerable<EvaluationDto>> GetByTeacherAsync(int teacherId);
}
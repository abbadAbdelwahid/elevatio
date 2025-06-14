using AnalyticsService.ExternalClients.DTO ;
namespace AnalyticsService.ExternalClients.ClientInterfaces;

public interface IEvaluationClient
{
    Task<IEnumerable<EvaluationDto>> GetByModuleAsync(int moduleId);

    /// <summary>
    /// Récupère toutes les évaluations associées à une filière.
    /// </summary>
    Task<IEnumerable<EvaluationDto>> GetByFiliereAsync(int filiereId); 


}
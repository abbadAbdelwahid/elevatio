using AnalyticsService.ExternalClients.DTO ;
namespace AnalyticsService.ExternalClients.ClientInterfaces;

public interface IQuestionnaireClient
{
    /// <summary>
    /// Récupère les questionnaires associés à un module.
    /// </summary>
    Task<IEnumerable<QuestionnaireDto>> GetByModuleAsync(int moduleId);

    /// <summary>
    /// Récupère les questionnaires associés à une filière.
    /// </summary>
    Task<IEnumerable<QuestionnaireDto>> GetByFiliereAsync(int filiereId); 
    Task<IEnumerable<QuestionnaireDto>> GetByIdAsync(int Id);
}
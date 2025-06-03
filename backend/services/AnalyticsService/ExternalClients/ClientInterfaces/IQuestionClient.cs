using AnalyticsService.ExternalClients.DTO ;
namespace AnalyticsService.ExternalClients.ClientInterfaces;

public interface IQuestionClient
{
    /// <summary>
    /// Récupère toutes les questions associées à un questionnaire donné.
    /// </summary>
    Task<IEnumerable<QuestionDto>> GetByQuestionnaireAsync(int questionnaireId);
}
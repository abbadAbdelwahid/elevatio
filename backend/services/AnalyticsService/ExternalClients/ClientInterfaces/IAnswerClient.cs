using AnalyticsService.ExternalClients.DTO ; 
namespace AnalyticsService.ExternalClients.ClientInterfaces;

public interface IAnswerClient
{
    /// <summary>
    /// Récupère toutes les réponses pour une question donnée.
    /// </summary>
    Task<IEnumerable<AnswerDto>> GetByQuestionAsync(int questionId);
}
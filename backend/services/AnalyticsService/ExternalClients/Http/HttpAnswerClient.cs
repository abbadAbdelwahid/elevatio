using AnalyticsService.ExternalClients.ClientInterfaces ;  
using AnalyticsService.ExternalClients.DTO;
namespace AnalyticsService.ExternalClients.Http;

public class HttpAnswerClient : IAnswerClient 
{
    private readonly HttpClient _http;
    public HttpAnswerClient(HttpClient http) => _http = http;

    
    // Récupérer les réponses d'une question donnée par ID
    public async Task<IEnumerable<AnswerDto>> GetByQuestionAsync(int questionId)
    {
        var response = await _http.GetAsync($"api/answers");
        response.EnsureSuccessStatusCode();
        var answers = await response.Content.ReadFromJsonAsync<IEnumerable<AnswerDto>>();

        // Retourne uniquement les réponses de la question spécifique
        return answers.Where(a => a.QuestionId == questionId);
    }

}
using AnalyticsService.ExternalClients.ClientInterfaces ;  
using AnalyticsService.ExternalClients.DTO;
namespace AnalyticsService.ExternalClients.Http; 
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;    // <-- pour ReadFromJsonAsync<T>
using System.Text.Json;
using System.Threading.Tasks; 

public class HttpAnswerClient : IAnswerClient 
{
    private readonly HttpClient _http;
    private IConfiguration cfg;
    private string _evaluationServiceBaseUrl;

    public HttpAnswerClient(HttpClient http, IConfiguration cfg)
    {
        _http = http; cfg = cfg; _evaluationServiceBaseUrl = cfg["EvaluationService:BaseUrl"];
    }


    // Récupérer les réponses d'une question donnée par ID
    public async Task<IEnumerable<AnswerDto>> GetByQuestionAsync(int questionId)
    {
        var response = await _http.GetAsync($"{_evaluationServiceBaseUrl}/api/answers/getAnswersByQuestionId/{questionId}");
        // Vérifie si la réponse est réussie
        if (!response.IsSuccessStatusCode)
        {
            // Log ou exception si nécessaire
            throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
        }

        // Lire et désérialiser la réponse JSON en une liste d'AnswerDto
        IEnumerable<AnswerDto> answers = null;
        try
        {
            answers = await response.Content.ReadFromJsonAsync<IEnumerable<AnswerDto>>();
        }
        catch (JsonException ex)
        {
            // Gérer l'exception en cas de problème de désérialisation
            throw new JsonException("Error deserializing the response content", ex);
        }

        // Assurez-vous que des réponses sont bien récupérées
        if (answers == null)
        {
            return Enumerable.Empty<AnswerDto>();
        }

        // Retourne uniquement les réponses de la question spécifique
        return answers.Where(a => a.QuestionId == questionId);
    }

}
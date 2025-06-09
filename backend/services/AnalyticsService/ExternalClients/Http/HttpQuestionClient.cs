using System.Text.Json;

namespace AnalyticsService.ExternalClients.Http;
using AnalyticsService.ExternalClients.ClientInterfaces ;  
using AnalyticsService.ExternalClients.DTO;

public class HttpQuestionClient : IQuestionClient
{
    private readonly HttpClient _http;
    private IConfiguration cfg;
    private String _evaluationServiceBaseUrl; 

    public HttpQuestionClient(HttpClient http, IConfiguration cfg)
    { 
        _http = http; cfg = cfg; _evaluationServiceBaseUrl = cfg["EvaluationService:BaseUrl"];
    }

    public async Task<IEnumerable<QuestionDto>> GetByQuestionnaireAsync(int questionnaireId)
    {   
        try{
        var response = await _http.GetAsync($"{_evaluationServiceBaseUrl}/api/questions/getQuestionsByQuestionnaireId/{questionnaireId}");
        // Vérifie si la réponse HTTP est réussie
        response.EnsureSuccessStatusCode();

        // Désérialisation du contenu JSON en une liste de QuestionDto
        var questions = await response.Content.ReadFromJsonAsync<IEnumerable<QuestionDto>>();

        if (questions == null)
        {
            // Retourne une liste vide si la réponse est null
            return Enumerable.Empty<QuestionDto>();
        }

        return questions;
        }
        catch (HttpRequestException ex)
        {
            // En cas d'erreur de la requête HTTP
            throw new Exception("La requête HTTP a échoué.", ex);
        }
        catch (JsonException ex)
        {
            // En cas d'erreur de désérialisation JSON
            throw new Exception("La désérialisation de la réponse JSON a échoué.", ex);
        }
        catch (Exception ex)
        {
            // Gérer toute autre exception inattendue
            throw new Exception("Une erreur inattendue s'est produite.", ex);
        }
    }}
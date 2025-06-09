using System.Text.Json;

namespace AnalyticsService.ExternalClients.Http;
using AnalyticsService.ExternalClients.ClientInterfaces ;  
using AnalyticsService.ExternalClients.DTO;
public class HttpEvaluationClient : IEvaluationClient 
{
    private readonly HttpClient _http;
    private IConfiguration _cfg;
    private String _evaluationServiceBaseUrl; 
    public HttpEvaluationClient(HttpClient http , IConfiguration cfg)
    {
        _http = http; cfg = cfg; _evaluationServiceBaseUrl = cfg["EvaluationService:BaseUrl"];
    }

    public async Task<IEnumerable<EvaluationDto>> GetByModuleAsync(int moduleId)
    {
        try
        {
            var response = await _http.GetAsync($"{_evaluationServiceBaseUrl}/api/evaluations/getEvaluationsByModuleId/{moduleId}");
            // Vérifie si la réponse HTTP est réussie
            response.EnsureSuccessStatusCode();

            // Désérialisation du contenu JSON en une liste d'EvaluationDto
            var evaluations = await response.Content.ReadFromJsonAsync<IEnumerable<EvaluationDto>>();

            if (evaluations == null)
            {
                // Retourne une liste vide si la réponse est null
                return Enumerable.Empty<EvaluationDto>();
            }

            return evaluations;
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
    }


    public async Task<IEnumerable<EvaluationDto>> GetByFiliereAsync(int filiereId)
    {
        try
        {
            var response = await _http.GetAsync($"{_evaluationServiceBaseUrl}/api/evaluations/getEvaluationsByFiliereId/{filiereId}");
            // Vérifie si la réponse HTTP est réussie
            response.EnsureSuccessStatusCode();

            // Désérialisation du contenu JSON en une liste d'EvaluationDto
            var evaluations = await response.Content.ReadFromJsonAsync<IEnumerable<EvaluationDto>>();

            if (evaluations == null)
            {
                // Retourne une liste vide si la réponse est null
                return Enumerable.Empty<EvaluationDto>();
            }

            return evaluations;
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
    }

    public async Task<IEnumerable<EvaluationDto>> GetByTeacherAsync(int teacherId)
    {
        var response = await _http.GetAsync($"api/evaluations?teacherId={teacherId}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<EvaluationDto>>();
    }
}
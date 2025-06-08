using System.Text.Json;

namespace AnalyticsService.ExternalClients.Http;
using AnalyticsService.ExternalClients.ClientInterfaces ;  
using AnalyticsService.ExternalClients.DTO; 

public class HttpQuestionnaireClient:IQuestionnaireClient
{
    private readonly HttpClient _http;
    private IConfiguration cfg;
    private String _evaluationServiceBaseUrl;  

    public HttpQuestionnaireClient(HttpClient http, IConfiguration cfg)
    {
        _http = http ?? throw new ArgumentNullException(nameof(http));
        cfg = cfg ?? throw new ArgumentNullException(nameof(cfg));
        _evaluationServiceBaseUrl = cfg["EvaluationService:BaseUrl"] ?? 
                                    throw new ArgumentException("EvaluationService BaseUrl is not configured");
    }

    public async Task<IEnumerable<QuestionnaireDto>> GetByIdAsync(int Id)
    {
        var requestUri =  $"{_evaluationServiceBaseUrl}/api/Questionnaire/getQuestionnaireById/{Id}" ; 
    
       
        var response = await _http.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();
        
        var quest = await response.Content.ReadFromJsonAsync<IEnumerable<QuestionnaireDto>>();
        return quest; 
    } 
    // Récupérer les questionnaires pour un module spécifique
    
    public async Task<IEnumerable<QuestionnaireDto>> GetByModuleAsync(int moduleId)
    {
        var requestUri =  $"{_evaluationServiceBaseUrl}/api/Questionnaire/getQuestionnairesByModuleId/{moduleId}" ; 
    
        // Récupérer tous les questionnaires de l'API externe 
        Console.WriteLine($"URL de la requête : {requestUri}"); 
        var response = await _http.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();

        var questionnaires = await response.Content.ReadFromJsonAsync<IEnumerable<QuestionnaireDto>>();

        // Appliquer un filtrage local sur moduleId
        return questionnaires.Where(q => q.ModuleId == moduleId);
    }

    // Récupérer les questionnaires pour une filière spécifique
    public async Task<IEnumerable<QuestionnaireDto>> GetByFiliereAsync(int filiereId)
    {
        try
        {
            // Récupérer tous les questionnaires de l'API externe
            var response =
                await _http.GetAsync(
                    $"{_evaluationServiceBaseUrl}/api/Questionnaire/getQuestionnairesByFiliereId/{filiereId}");
            // Vérifie si la réponse HTTP est réussie (code 2xx)
            response.EnsureSuccessStatusCode();

            // Désérialisation de la réponse JSON en une liste de QuestionnaireDto
            var questionnaires = await response.Content.ReadFromJsonAsync<IEnumerable<QuestionnaireDto>>();

            // Si la réponse est null, retourner une liste vide
            if (questionnaires == null)
            {
                return Enumerable.Empty<QuestionnaireDto>();
            }

            // Appliquer un filtrage local sur filiereId (bien que cela devrait être déjà filtré par l'API)
            return questionnaires.Where(q => q.FiliereId == filiereId);
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
    
    } 

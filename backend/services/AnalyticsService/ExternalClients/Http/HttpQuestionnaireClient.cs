namespace AnalyticsService.ExternalClients.Http;
using AnalyticsService.ExternalClients.ClientInterfaces ;  
using AnalyticsService.ExternalClients.DTO;

public class HttpQuestionnaireClient:IQuestionnaireClient
{
    private readonly HttpClient _http;
    public HttpQuestionnaireClient(HttpClient http) => _http = http;

    // Récupérer les questionnaires pour un module spécifique
    public async Task<IEnumerable<QuestionnaireDto>> GetByModuleAsync(int moduleId)
    {
        // Récupérer tous les questionnaires de l'API externe
        var response = await _http.GetAsync("api/questionnaires");
        response.EnsureSuccessStatusCode();

        var questionnaires = await response.Content.ReadFromJsonAsync<IEnumerable<QuestionnaireDto>>();

        // Appliquer un filtrage local sur moduleId
        return questionnaires.Where(q => q.ModuleId == moduleId);
    }

    // Récupérer les questionnaires pour une filière spécifique
    public async Task<IEnumerable<QuestionnaireDto>> GetByFiliereAsync(int filiereId)
    {
        // Récupérer tous les questionnaires de l'API externe
        var response = await _http.GetAsync("/api/questionnaires");
        response.EnsureSuccessStatusCode();

        var questionnaires = await response.Content.ReadFromJsonAsync<IEnumerable<QuestionnaireDto>>();

        // Appliquer un filtrage local sur filiereId
        return questionnaires.Where(q => q.FiliereId == filiereId);
    }
}
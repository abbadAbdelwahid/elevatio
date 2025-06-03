namespace AnalyticsService.ExternalClients.Http;
using AnalyticsService.ExternalClients.ClientInterfaces ;  
using AnalyticsService.ExternalClients.DTO;
public class HttpEvaluationClient : IEvaluationClient 
{
    private readonly HttpClient _http;
    public HttpEvaluationClient(HttpClient http) => _http = http;

    public async Task<IEnumerable<EvaluationDto>> GetByModuleAsync(int moduleId)
    {
        var response = await _http.GetAsync($"/api/evaluations");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<EvaluationDto>>();
    }


    public async Task<IEnumerable<EvaluationDto>> GetByFiliereAsync(int filiereId)
    {
        var response = await _http.GetAsync($"/api/evaluations?filiereId={filiereId}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<EvaluationDto>>();
    }

    public async Task<IEnumerable<EvaluationDto>> GetByTeacherAsync(int teacherId)
    {
        var response = await _http.GetAsync($"/api/evaluations?teacherId={teacherId}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<EvaluationDto>>();
    }
}
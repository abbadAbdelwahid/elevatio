namespace AnalyticsService.ExternalClients.Http;
using AnalyticsService.ExternalClients.ClientInterfaces ;  
using AnalyticsService.ExternalClients.DTO;

public class HttpQuestionClient : IQuestionClient
{
    private readonly HttpClient _http;
    public HttpQuestionClient(HttpClient http) => _http = http;

    public async Task<IEnumerable<QuestionDto>> GetByQuestionnaireAsync(int questionnaireId)
    {
        var response = await _http.GetAsync($"api/questions");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<QuestionDto>>();
    }}
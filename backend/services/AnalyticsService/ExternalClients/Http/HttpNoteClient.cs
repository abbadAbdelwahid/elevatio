using AnalyticsService.ExternalClients.ClientInterfaces;
using AnalyticsService.ExternalClients.DTO;

namespace AnalyticsService.ExternalClients.Http;

public class HttpNoteClient : INoteClient
{
    private readonly HttpClient _http;
    private IConfiguration _cfg;
    private String _CourseManagementServiceBaseUrl;

    public HttpNoteClient(HttpClient http, IConfiguration cfg)
    {
        _http = http;
        _cfg = cfg;
        _CourseManagementServiceBaseUrl = cfg["CourseManagementService:BaseUrl"]; 
        
    }

    public async Task<IEnumerable<NoteDto>> GetByModuleAsync(int moduleId)
    {
        string requestUri = $"{_CourseManagementServiceBaseUrl}/api/note/module/{moduleId}";

        // 2) Appel GET vers l'API externe
        var response = await _http.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();  // lève si 4xx/5xx

        // 3) Désérialiser le JSON en IEnumerable<NoteDto>
        var notes = await response.Content.ReadFromJsonAsync<IEnumerable<NoteDto>>();

        // 4) Sécurité : ne jamais retourner null
        return notes ?? Array.Empty<NoteDto>();
    } 
    public async Task<IEnumerable<NoteDto>> GetByEtdAsync(int StudentId)
    {
        string requestUri = $"{_CourseManagementServiceBaseUrl}/api/note/student/{StudentId}";

        // 2) Appel GET vers l'API externe
        var response = await _http.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();  // lève si 4xx/5xx

        // 3) Désérialiser le JSON en IEnumerable<NoteDto>
        var notes = await response.Content.ReadFromJsonAsync<IEnumerable<NoteDto>>();

        // 4) Sécurité : ne jamais retourner null
        return notes ?? Array.Empty<NoteDto>();
    } 
    public async Task<double> GetStdAverage(int StudentId) { 
        //api/note/student/{studentId}/average 
        string requestUri = $"{_CourseManagementServiceBaseUrl}/api/note/student/{StudentId}/average ";

        // 2) Appel GET vers l'API externe
        var response = await _http.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();  // lève si 4xx/5xx

        // 4) Sécurité : ne jamais retourner null
        return response.Content.ReadFromJsonAsync<double>().Result;
    }

    public async Task<IEnumerable<NoteDto>> GetNotesModule(int ModuleId)
    {
        string requestUri = $"{_CourseManagementServiceBaseUrl}/api/note/module/{ModuleId}";

        // 2) Appel GET vers l'API externe
        var response = await _http.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();  // lève si 4xx/5xx

        // 3) Désérialiser le JSON en IEnumerable<NoteDto>
        var notes = await response.Content.ReadFromJsonAsync<IEnumerable<NoteDto>>();

        // 4) Sécurité : ne jamais retourner null
        return notes ?? Array.Empty<NoteDto>();  
    } 
    
}
using System.Text.Json;
using AnalyticsService.ExternalClients.ClientInterfaces;
using AnalyticsService.ExternalClients.DTO;

namespace AnalyticsService.ExternalClients.Http;

public class HttpFiliereClient : IFiliereClient
{
    private readonly HttpClient _http;
    private IConfiguration _cfg;
    private String _CourseManagementServiceBaseUrl;

    public HttpFiliereClient(HttpClient http, IConfiguration cfg)
    {
        _http = http; cfg = cfg; _CourseManagementServiceBaseUrl = cfg["CourseManagementService:BaseUrl"];
    }

    public async Task<FiliereDto> GetFiliereByIdAsync(int FiliereId)
    { 
        // 1) Envoi de la requête HTTP GET
        var response = await _http.GetAsync($"{_CourseManagementServiceBaseUrl}/api/filiere/{FiliereId}");

        // 2) Vérifie si la réponse HTTP est réussie (2xx)
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
        }

        // 3) Désérialiser le JSON en FiliereDto
        FiliereDto filiere = null;
        try
        {
            filiere = await response.Content.ReadFromJsonAsync<FiliereDto>();
        }
        catch (JsonException ex)
        {
            throw new JsonException("Error deserializing the response content for FiliereDto", ex);
        }

        // 4) Vérifier que la filière n'est pas null (introuvable)
        if (filiere == null)
        {
            throw new KeyNotFoundException($"Filière avec l'ID {FiliereId} introuvable.");
        }

        // 5) Retourne l'objet FiliereDto
        return filiere;
    }
    }


    
   

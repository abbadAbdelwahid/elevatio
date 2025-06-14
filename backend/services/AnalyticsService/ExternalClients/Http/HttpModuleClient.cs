using System.Text.Json;
using AnalyticsService.ExternalClients.ClientInterfaces;
using AnalyticsService.ExternalClients.DTO;

namespace AnalyticsService.ExternalClients.Http;

public class HttpModuleClient : IModuleClient
{
    private readonly HttpClient _http;
    private IConfiguration _cfg;
    private String _CourseManagementServiceBaseUrl;

    public HttpModuleClient(HttpClient http, IConfiguration cfg)
    {
        _http = http;
        _cfg = cfg;
        _CourseManagementServiceBaseUrl = cfg["CourseManagementService:BaseUrl"];
    }

    public async Task<ModuleDto> GetModuleByIdAsync(int ModuleId)
    {
        // 1) Construction de l'URL d'appel au microservice Modules
        //    Exemple : http://localhost:5201/api/modules/{moduleId}
        string requestUrl = $"{_CourseManagementServiceBaseUrl}/api/module/{ModuleId}";

        // 2) Envoi de la requête GET
        HttpResponseMessage response = await _http.GetAsync(requestUrl);

        // 3) Vérifie si la réponse HTTP est réussie (2xx)
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
        }

        // 4) Désérialiser le contenu JSON en ModuleDto
        ModuleDto module = null;
        try
        {
            module = await response.Content.ReadFromJsonAsync<ModuleDto>();
        }
        catch (JsonException ex)
        {
            throw new JsonException($"Error deserializing the response content for ModuleDto (Id = {ModuleId})", ex);
        }

        // 5) Vérifier que le module n'est pas null (introuvable)
        if (module == null)
        {
            throw new KeyNotFoundException($"Module avec l'ID {ModuleId} introuvable.");
        }

        // 6) Retourne l'objet ModuleDto
        return module;
    }
    public async Task<IEnumerable<ModuleDto>> GetByFiliereAsync(string FiliereName)
    {
        // 1) Construire l'URL de l'endpoint
        var requestUrl = $"{_CourseManagementServiceBaseUrl}/api/Module/filiere/{FiliereName}";

        // 2) Envoyer la requête GET
        var response = await _http.GetAsync(requestUrl);

        // 3) Lire la réponse en tant que chaîne pour diagnostiquer plus facilement
        var responseContent = await response.Content.ReadAsStringAsync();
       
        // 4) Vérifier si le statut HTTP est correct (200 OK)
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"Request to {requestUrl} failed with status {response.StatusCode}");
        }

        try
        {
            // 5) Désérialisation manuelle (si nécessaire)
            var modules = JsonSerializer.Deserialize<IEnumerable<ModuleDto>>(responseContent);

            // Vérification si la désérialisation a échoué
            if (modules == null)
            {
                throw new InvalidOperationException("La désérialisation des modules a échoué.");
            }

            // 6) Retourner les modules, ou une liste vide si aucun module trouvé
            return modules.Any() ? modules : Array.Empty<ModuleDto>();
        }
        catch (JsonException ex)
        {
            // Logging des erreurs de désérialisation
            Console.WriteLine($"Erreur lors de la désérialisation des modules: {ex.Message}");
            throw new InvalidOperationException("Erreur lors de la désérialisation des modules", ex);
        }
        catch (Exception ex)
        {
            // Gestion des autres exceptions
            Console.WriteLine($"Erreur inattendue: {ex.Message}");
            throw new InvalidOperationException("Une erreur inattendue est survenue", ex);
        }
    }

    public async Task<IEnumerable<ModuleDto>> GetModuleByTeacherAsync(int teacherId)
    {
        // 1) Construire l'URL de l'endpoint
        var requestUrl = $"{_CourseManagementServiceBaseUrl}/api/module/teacher/{teacherId}";

        // 2) Envoyer la requête GET
        var response = await _http.GetAsync(requestUrl); 
        var responseContent = await response.Content.ReadAsStringAsync();
        Console.WriteLine("Response content: " + responseContent); 

        // 3) Vérifier que le status est 2xx
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"Request to {requestUrl} failed with status {response.StatusCode}");
        }

        try
        {
            // 4) Désérialiser en IEnumerable<ModuleDto>
            var modules = await response.Content
                .ReadFromJsonAsync<IEnumerable<ModuleDto>>();

            // 5) Ne jamais retourner null
            return modules ?? Array.Empty<ModuleDto>();
        }
        catch (JsonException ex)
        {
            // Logging si besoin
            throw new InvalidOperationException(
                "Erreur lors de la désérialisation des modules", ex);
        }
    }

   
      
    } 


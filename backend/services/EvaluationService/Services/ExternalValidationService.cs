// ExternalValidationService.cs
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using EvaluationService.Data;
using EvaluationService.Repositories;

namespace EvaluationService.Services;

public class ExternalValidationService : IExternalValidationService
{
    private readonly HttpClient _client;
    private readonly EvaluationsDbContext _dbContext;
    private readonly string? _authServiceEndpoint;
    private readonly string? _coursesManagementServiceEndpoint;
    private const string MediaType = "application/json";

    public ExternalValidationService(
        HttpClient client, 
        EvaluationsDbContext dbContext, 
        IConfiguration cfg,
        IEvaluationRepository evaluationRepository,
        IQuestionnaireRepository questionnaireRepository)
    {
        _client = client;
        _dbContext = dbContext;
        _authServiceEndpoint = cfg["Other-Microservices-EndPoints:AuthService-EndPoint"];
        _coursesManagementServiceEndpoint = cfg["Other-Microservices-EndPoints:CoursesManagementService-EndPoint"];
    }
    
    // Implémentation de la méthode UserExistsAsync de l'interface
    public async Task<bool> UserExistsAsync(string userId)
    {
        var userExistsDict = await UsersExistAsync(new List<string?> { userId });
        return userExistsDict.GetValueOrDefault(userId, false);
    }

    // Méthode utilitaire pour vérifier l'existence de plusieurs utilisateurs
    public async Task<Dictionary<string,bool>> UsersExistAsync(List<string?> userIds)
    {
        try
        {
            var url = $"{_authServiceEndpoint}/api/external-validation/users-ids-exist";
            var json = JsonSerializer.Serialize(userIds);
            var content = new StringContent(json, Encoding.UTF8, MediaType);

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = content
            };

            var response = await _client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseContent);
                return new Dictionary<string, bool>();
            }

            var responseStream = await response.Content.ReadAsStreamAsync();
            var existenceDict = JsonSerializer.Deserialize<Dictionary<string, bool>>(responseStream);
            return existenceDict ?? new Dictionary<string, bool>();
        }
        catch (Exception ex) when(ex is HttpRequestException or TaskCanceledException)
        {
            throw new Exception("server is down or not reachable", ex);
        }
    }

    // Implémentation de la méthode FiliereExistsAsync de l'interface
    public async Task<Dictionary<int, bool>> FilieresExistAsync(List<int?> filiereIds)
    {
        try
        {
            var url = $"{_coursesManagementServiceEndpoint}/api/external-validation/filieres-ids-exist";
            var json = JsonSerializer.Serialize(filiereIds);
            var content = new StringContent(json, Encoding.UTF8, MediaType);
            
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = content
            };
            
            var response = await _client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseContent);
                return new Dictionary<int, bool>();
                
            }
            var responseStream = await response.Content.ReadAsStreamAsync();
            var existenceDict = JsonSerializer.Deserialize<Dictionary<int, bool>>(responseStream);
            return existenceDict ?? new Dictionary<int, bool>();
        }
        catch (Exception ex) when(ex is HttpRequestException or TaskCanceledException)
        {
            throw new Exception("Server is down or unreachable");
        }
    }
    
    public async Task<bool> FiliereExistsAsync(int filiereId)
    {
        var filiereIdDict = await FilieresExistAsync(new List<int?> { filiereId });
        return filiereIdDict.GetValueOrDefault(filiereId, false);
    }
    
    public async  Task<Dictionary<int, bool>> ModulesExistAsync(List<int?> moduleIds)
    {
        try
        {
            var url = $"{_coursesManagementServiceEndpoint}/api/external-validation/modules-ids-exist";
            var json = JsonSerializer.Serialize(moduleIds);
            var content = new StringContent(json, Encoding.UTF8, MediaType);
            
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = content
            };
            
            var response = await _client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseContent);
                return new Dictionary<int, bool>();
            }

            var responseStream = await response.Content.ReadAsStreamAsync();
            var existenceDict = JsonSerializer.Deserialize<Dictionary<int, bool>>(responseStream);
            return existenceDict ?? new Dictionary<int, bool>();
        }
        catch (Exception ex) when(ex is HttpRequestException or TaskCanceledException)
        {
            throw new Exception("Server is down or unreachable", ex);
        }
    }
    
    public async Task<bool> ModuleExistsAsync(int moduleId)
    {
        var moduleIdDict = await ModulesExistAsync(new List<int?> { moduleId });
        return moduleIdDict.GetValueOrDefault(moduleId, false);
    }
}
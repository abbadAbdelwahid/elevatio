namespace AnalyticsService.ExternalClients.OpenAI;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers; 
using AnalyticsService.ExternalClients.OpenAI; 
using System.Text.Json ;

public class GroqAIClient : IGroqAIClient
{
    private readonly HttpClient _http;
    private readonly string? _apiKey;
    private IConfiguration _cfg;

    public GroqAIClient(IOptions<GroqAIOptions> options, HttpClient http, IConfiguration cfg)
    {
        _http = http;
        _cfg = cfg;
        // Récupération de la clé GroqAI : options.Configuration ou variable d'environnement
        var keyFromOptions = options.Value.ApiKey;
        var keyFromEnv = _cfg.GetValue<string>("Groq:ApiKey");
        _apiKey = keyFromOptions ?? keyFromEnv
            ?? throw new InvalidOperationException("GroqAI API key is not configured.");
        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _apiKey);
    }

    public async Task<string> SendChatAsync(string prompt)
    {
        var payload = new
        {
            model = "meta-llama/llama-4-scout-17b-16e-instruct",
            temperature = 0.3,
            messages = new[]
            {

                new { role = "system", content = "Tu es un expert en pédagogie et analyse de données." },
                new { role = "user", content = prompt }
            }
        };

        try
        {
            var response = await _http.PostAsJsonAsync("https://api.groq.com/openai/v1/chat/completions", payload);

            // Vérification si la réponse est réussie
            response.EnsureSuccessStatusCode();

            // Lire le contenu de la réponse
            var responseContent = await response.Content.ReadAsStringAsync();

            // Analyser la réponse JSON
            using var doc = JsonDocument.Parse(responseContent);

            // Vérification de l'existence des propriétés avant d'y accéder
            if (doc.RootElement.TryGetProperty("choices", out var choices) &&
                choices[0].TryGetProperty("message", out var message))
            {
                return message.GetProperty("content").GetString();
            }
            else
            {
                throw new Exception("Réponse malformée : Propriétés attendues non trouvées.");
            }
        }
        catch (HttpRequestException e)
        {
            // Gestion d'erreur pour les problèmes réseau
            throw new Exception("Erreur lors de l'appel à l'API : " + e.Message);
        }
        catch (JsonException e)
        {
            // Gestion des erreurs liées à la désérialisation JSON
            throw new Exception("Erreur lors du traitement de la réponse JSON : " + e.Message);
        }
        catch (Exception e)
        {
            // Gestion d'autres erreurs générales
            throw new Exception("Une erreur est survenue : " + e.Message);
        }
    }
} 
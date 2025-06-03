namespace AnalyticsService.ExternalClients.OpenAI;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers; 
using AnalyticsService.ExternalClients.OpenAI; 
using System.Text.Json ; 
public class OpenAIClient : IOpenAIClient
{
    private readonly HttpClient _http;
    private readonly string? _apiKey;

    public OpenAIClient(IOptions<OpenAIOptions> options, HttpClient http)
    {
        _http   = http;
        // Récupération de la clé OpenAI : options.Configuration ou variable d'environnement
        var keyFromOptions = options.Value.ApiKey;
        var keyFromEnv     = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
        _apiKey = keyFromOptions ?? keyFromEnv
            ?? throw new InvalidOperationException("OpenAI API key is not configured."); 
        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _apiKey);
    }

    public async Task<string> SendChatAsync(string prompt)
    {
        var payload = new
        {
            model    = "gpt-4",
            messages = new[] {
                new { role = "system", content = "Tu es un expert en pédagogie et analyse de données." },
                new { role = "user", content = prompt }
            }
        };

        var response = await _http.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", payload);
        response.EnsureSuccessStatusCode();

        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        return doc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();
    }
}
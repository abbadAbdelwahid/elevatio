namespace AnalyticsService.ExternalClients.OpenAI;

public interface IOpenAIClient
{
    Task<string> SendChatAsync(string prompt); 
    
    
}
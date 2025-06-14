namespace AnalyticsService.ExternalClients.OpenAI;
using System;
public interface IGroqAIClient
{
    Task<string> SendChatAsync(string prompt); 
    
    
}
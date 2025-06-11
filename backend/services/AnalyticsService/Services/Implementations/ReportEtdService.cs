using AnalyticsService.Data;
using AnalyticsService.ExternalClients.ClientInterfaces;
using AnalyticsService.ExternalClients.OpenAI;
using AnalyticsService.Services.Interfaces;

namespace AnalyticsService.Services.Implementations;

public class ReportEtdService
{ 
    private readonly IGroqAIClient _groqAi;
    private readonly AnalyticsDbContext _db;
    private readonly IAnswerClient _ansClient;
    private readonly IQuestionClient _quesClient;
    private readonly IFiliereClient _filiereClient;
    private readonly IModuleClient _moduleClient; 
    private readonly IQuestionnaireClient _questionnaireClient;  
    private readonly INoteClient _noteClient;  

    
    public ReportEtdService(IGroqAIClient groqAi, AnalyticsDbContext db, IAnswerClient ansClient,
        IQuestionClient quesClient, IModuleClient moduleClient, IFiliereClient filiereClient,INoteClient noteClient)
    {
        _groqAi = groqAi;
        _db = db;
        _ansClient = ansClient;
        _quesClient = quesClient;
        _moduleClient = moduleClient;
        _filiereClient = filiereClient;
        _noteClient = noteClient;
    }

   
}
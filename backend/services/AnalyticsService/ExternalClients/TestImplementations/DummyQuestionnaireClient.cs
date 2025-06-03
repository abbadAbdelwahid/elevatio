namespace AnalyticsService.ExternalClients.TestImplementations;
using AnalyticsService.ExternalClients.DTO; 
using AnalyticsService.ExternalClients.ClientInterfaces;
public class DummyQuestionnaireClient : IQuestionnaireClient
{
    private readonly IEnumerable<QuestionnaireDto> _data;
    public DummyQuestionnaireClient(IEnumerable<QuestionnaireDto> data) 
        => _data = data;
    public Task<IEnumerable<QuestionnaireDto>> GetByModuleAsync(int moduleId)
        => Task.FromResult(_data.Where(q => q.ModuleId == moduleId));
    public Task<IEnumerable<QuestionnaireDto>> GetByFiliereAsync(int filiereId)
        => Task.FromResult(_data.Where(q => q.FiliereId == filiereId));
}
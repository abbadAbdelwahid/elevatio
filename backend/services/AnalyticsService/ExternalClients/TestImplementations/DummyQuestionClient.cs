namespace AnalyticsService.ExternalClients.TestImplementations;
using AnalyticsService.ExternalClients.DTO; 
using AnalyticsService.ExternalClients.ClientInterfaces;
public class DummyQuestionClient : IQuestionClient
{
    private readonly IEnumerable<QuestionDto> _data;

    public DummyQuestionClient(IEnumerable<QuestionDto> data) =>
        _data = data; 

    public Task<IEnumerable<QuestionDto>> GetByQuestionnaireAsync(int questionnaireId) =>
        Task.FromResult(_data.Where(q => q.QuestionnaireId == questionnaireId));
}
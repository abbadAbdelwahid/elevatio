namespace AnalyticsService.ExternalClients.TestImplementations;
using AnalyticsService.ExternalClients.DTO; 
using AnalyticsService.ExternalClients.ClientInterfaces;
public class DummyAnswerClient : IAnswerClient
{
    private readonly IEnumerable<AnswerDto> _data;

    public DummyAnswerClient(IEnumerable<AnswerDto> data) =>
        _data = data;

    public Task<IEnumerable<AnswerDto>> GetByQuestionAsync(int questionId) =>
        Task.FromResult(_data.Where(a => a.QuestionId == questionId));
}
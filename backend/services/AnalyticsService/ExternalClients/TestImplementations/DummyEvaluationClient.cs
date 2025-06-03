namespace AnalyticsService.ExternalClients.TestImplementations;
using AnalyticsService.ExternalClients.DTO; 
using AnalyticsService.ExternalClients.ClientInterfaces;
public class DummyEvaluationClient : IEvaluationClient
{
    private readonly IEnumerable<EvaluationDto> _data;

    public DummyEvaluationClient(IEnumerable<EvaluationDto> data) =>
        _data = data;

    public Task<IEnumerable<EvaluationDto>> GetByModuleAsync(int moduleId) =>
        Task.FromResult(_data.Where(e => e.ModuleId == moduleId));

    public Task<IEnumerable<EvaluationDto>> GetByFiliereAsync(int filiereId) =>
        Task.FromResult(_data.Where(e => e.FiliereId == filiereId));

 
}
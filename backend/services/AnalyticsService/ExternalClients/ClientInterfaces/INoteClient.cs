using AnalyticsService.ExternalClients.DTO;
using System;
using AnalyticsService.ExternalClients.DTO;
namespace AnalyticsService.ExternalClients.ClientInterfaces;

public interface INoteClient
{
    public Task<IEnumerable<NoteDto>> GetByModuleAsync(int moduleId); 
    public Task<IEnumerable<NoteDto>> GetByEtdAsync(int etdId );
    public Task<double> GetStdAverage(int StudentId);
    public Task<IEnumerable<NoteDto>> GetNotesModule(int ModuleId); 


}
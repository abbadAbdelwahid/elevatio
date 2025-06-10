using AnalyticsService.ExternalClients.DTO;

namespace AnalyticsService.ExternalClients.ClientInterfaces;

public interface IModuleClient
{ 
    Task<ModuleDto> GetModuleByIdAsync(int ModuleId); 
    Task<IEnumerable<ModuleDto>> GetModuleByTeacherAsync(int ModuleId);  
    Task<IEnumerable<ModuleDto>> GetByFiliereAsync(String FiliereName);  
}     
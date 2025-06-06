using AnalyticsService.ExternalClients.DTO;

namespace AnalyticsService.ExternalClients.ClientInterfaces;

public interface IFiliereClient
{
    Task<FiliereDto> GetFiliereByIdAsync(int FiliereId);
}
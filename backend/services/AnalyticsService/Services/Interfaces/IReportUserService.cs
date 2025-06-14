namespace AnalyticsService.Services.Interfaces;

public interface IReportUserService
{
    Task<byte[]> GenerateUserPerformanceReport(int UserId); 
    Task<string> GenerateUserPerformanceReporthtml(int UserId);
    
}
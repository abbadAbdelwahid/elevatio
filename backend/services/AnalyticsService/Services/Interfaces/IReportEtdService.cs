namespace AnalyticsService.Services.Interfaces;

public interface IReportEtdService
{
    Task<byte[]> GenerateUserPerformanceReport(int UserId); 
    Task<string> GenerateUserPerformanceReporthtml(int UserId);

}
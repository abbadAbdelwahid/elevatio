namespace AnalyticsService.Services.Interfaces;

public interface IReportUserService
{
    Task<byte[]> GenerateUserPerformanceReport(int UserId);
    Task<byte[]> GenerateEnsReportPassAvgdfAsync(int teacherId); 
}
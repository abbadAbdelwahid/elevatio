using Microsoft.AspNetCore.Mvc;

namespace AnalyticsService.Services.Interfaces;

public interface IReportPropertyService
{
    Task<byte[]> GenerateModuleReportPdfAsync(int moduleId); 
    Task<byte[]> GenerateFiliereReportPdfAsync(int FiliereId); 
   //  Task<byte[]> GenerateEnsReportPdfAsync(int EnsId);
    Task<byte[]> GenerateFQReportPdfAsync(int filiereId, int questionnaireId);
    Task<byte[]> GenerateMQReportPdfAsync(int moduleId, int questionnaireId); 
}
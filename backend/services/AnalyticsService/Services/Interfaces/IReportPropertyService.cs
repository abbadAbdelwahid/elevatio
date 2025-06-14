using Microsoft.AspNetCore.Mvc;

namespace AnalyticsService.Services.Interfaces;

public interface IReportPropertyService
{
    Task<byte[]> GenerateModuleReportPdfAsync(int moduleId);  
    Task<string> GenerateModuleReportPdfAsyncHtml(int moduleId);
    
    Task<byte[]> GenerateFiliereReportPdfAsync(int FiliereId); 
    Task<string> GenerateFiliereReportPdfAsyncHtml(int FiliereId);  
   //  Task<byte[]> GenerateEnsReportPdfAsync(int EnsId);
    
}
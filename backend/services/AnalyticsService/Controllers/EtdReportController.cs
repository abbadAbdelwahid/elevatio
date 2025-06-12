using AnalyticsService.Services.Interfaces;

namespace AnalyticsService.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;  
[Route("api/[controller]")]
[ApiController]
public class EtdReportController : ControllerBase
{
    private readonly IReportEtdService _reportEtdService;

    public EtdReportController(IReportEtdService reportEtdService)
    {
        _reportEtdService = reportEtdService; 
 
    }  
    [HttpGet("generate-PerformanceReport/{etdId}")]
    public async Task<IActionResult> GeneratePerformanceReport(int etdId)
    {
        try
        {
            // Appeler la méthode pour générer le rapport
            byte[] reportData = await _reportEtdService.GenerateUserPerformanceReport(etdId);
    
            // Retourner le fichier généré (ici, on suppose que c'est un PDF)
            return File(reportData, "application/pdf", $"rapport_performance_etudiant_Id{etdId}.pdf");
        }
        catch (Exception ex)
        {
            // Gérer les erreurs (par exemple, si l'enseignant n'existe pas ou s'il y a un problème de génération)
            return BadRequest($"Une erreur est survenue lors de la génération du rapport : {ex.Message}");
        }
    }
    
}
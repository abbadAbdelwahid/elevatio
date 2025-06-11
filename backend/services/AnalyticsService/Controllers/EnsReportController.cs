using AnalyticsService.Services.Interfaces;

namespace AnalyticsService.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;  
[Route("api/[controller]")]
[ApiController]
public class EnsReportController : ControllerBase
{
    private readonly IReportUserService _reportUserService;

    public EnsReportController(IReportUserService reportUserService)
    {
        _reportUserService = reportUserService; 

    }   
     [HttpGet("generate-PrerformanceReport/{teacherId}")]
        public async Task<IActionResult> GeneratePerformanceReport(int teacherId)
        {
            try
            {
                // Appeler la méthode pour générer le rapport
                byte[] reportData = await _reportUserService.GenerateEnsReportPassAvgdfAsync(teacherId);
    
                // Retourner le fichier généré (ici, on suppose que c'est un PDF)
                return File(reportData, "application/pdf", $"rapport_performance_enseignant_Id{teacherId}.pdf");
            }
            catch (Exception ex)
            {
                // Gérer les erreurs (par exemple, si l'enseignant n'existe pas ou s'il y a un problème de génération)
                return BadRequest($"Une erreur est survenue lors de la génération du rapport : {ex.Message}");
            }
        }
}
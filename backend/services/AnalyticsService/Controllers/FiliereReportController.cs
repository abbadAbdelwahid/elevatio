using AnalyticsService.Services.Interfaces;

namespace AnalyticsService.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks; 
using AnalyticsService.Services.Implementations; 
[Route("api/[controller]")]
[ApiController]

public class FiliereReportController: ControllerBase
{
    private readonly IReportService _reportService;

    public FiliereReportController(IReportService reportService)
    {
        _reportService = reportService;
        
    }  
    // POST api/moduleReport/generate
    [HttpPost("generate/{FiliereID}")]
    public async Task<IActionResult> GenerateModuleReport(int FiliereID )
    {
        try
        {
            var pdfBytes = await _reportService.GenerateFiliereReportPdfAsync(FiliereID);
            return File(pdfBytes, "application/pdf", $"FiliereReport{FiliereID}.pdf");
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }   
    // POST api/rapportquestionnaire/generate
    [HttpPost("generate/{FiliereId}/{QuestionnaireId}")]
    public async Task<IActionResult> GenerateRapportQuestionnaire(int FiliereId, int QuestionnaireId)
    {
        try
        {
            // 1) Utiliser le service pour générer le rapport et l'enregistrer dans la base de données
            var pdfBytes = await _reportService.GenerateFQReportPdfAsync(FiliereId, QuestionnaireId);

            // 2) Retourner le PDF généré
            return File(pdfBytes, "application/pdf", $"RapportQuestionnaire_{QuestionnaireId}_Filiere_{FiliereId}.pdf");
        }
        catch (Exception ex)
        {
            // En cas d'erreur, retourner un message d'erreur
            return StatusCode(500, new { message = ex.Message });
        }
    }
}
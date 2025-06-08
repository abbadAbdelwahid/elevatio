using AnalyticsService.Services.Interfaces;

namespace AnalyticsService.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks; 
using AnalyticsService.Services.Implementations; 
[Route("api/[controller]")]
[ApiController]
public class ModuleReportController : ControllerBase
{
    private readonly IReportService _reportService;

    public ModuleReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        // POST api/moduleReport/generate
        [HttpPost("generate/{ModuleId}")]
        public async Task<IActionResult> GenerateModuleReport(int ModuleId )
        {
            try
            {
                var pdfBytes = await _reportService.GenerateModuleReportPdfAsync(ModuleId); 
                // Si le PDF est vide ou null, on renvoie 404
                if (pdfBytes == null || pdfBytes.Length == 0)
                    return NotFound("Le PDF est vide ou introuvable.");
                return File(pdfBytes, "application/pdf", $"ModuleReport{ModuleId}.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }  
        // POST api/rapportquestionnaire/generate
        [HttpPost("generate/{ModuleId}/{QuestionId}")]
        public async Task<IActionResult> GenerateRapportQuestionnaire(int ModuleId, int QuestionnaireId)
        {
            try
            {
                // 1) Utiliser le service pour générer le rapport et l'enregistrer dans la base de données
                var pdfBytes = await _reportService.GenerateMQReportPdfAsync(ModuleId, QuestionnaireId);

                // 2) Retourner le PDF généré
                return File(pdfBytes, "application/pdf", $"RapportQuestionnaire_{QuestionnaireId}_Module_{ModuleId}.pdf");
            }
            catch (Exception ex)
            {
                // En cas d'erreur, retourner un message d'erreur
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }

   

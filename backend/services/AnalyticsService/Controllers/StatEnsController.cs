namespace AnalyticsService.Controllers;
using Microsoft.AspNetCore.Mvc ;  
using AnalyticsService.Services.Interfaces;
using AnalyticsService.Models; 
[ApiController]
[Route("api/[controller]")]
public class StatEnsController : ControllerBase
{
    private readonly IStatistiqueUserService<StatistiqueEnseignant> _service;  
    public StatEnsController(IStatistiqueUserService<StatistiqueEnseignant> svc)
        => _service = svc;
    [HttpGet("{id}/Refresh")]
    public async Task<ActionResult<StatistiqueEnseignant>> RefreshMarksStats(int teacherId)
    {
        try
        {
            // Appel de la méthode qui recharge et met à jour les stats en base
            var stats = await _service.CalculateStats(teacherId);
            return Ok(stats);
        }
        catch (KeyNotFoundException knf)
        {
            // Si pas de stat existante pour cet enseignant
            return NotFound(knf.Message);
        }
        catch (InvalidOperationException ioe)
        {
            // Si pas de modules ou stats de modules associés
            return BadRequest(ioe.Message);
        }
        catch (Exception ex)
        {
            // Erreur inattendue
            // (log ex ici si vous avez un logger)
            return StatusCode(500, "Une erreur est survenue : " + ex.Message);
        }
    }
    [HttpPost("Create/{EnsId}")]
    public async Task<IActionResult> Create(int EnsId)
    {
        var stat = await _service.CreateAsync(EnsId);
        return Ok(stat); 
    } 
    [HttpGet("{ensId}")]
    public async Task<IActionResult> GetByEnseignant(int ensId)
    {
        try
        {
            var stats = await _service.GetByUserIdAsync(ensId);
            return Ok(stats);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { Message = $"Aucune statistique trouvée pour l'enseignant {ensId}." });
        }
    }
}
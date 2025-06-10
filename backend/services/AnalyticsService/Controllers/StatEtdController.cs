namespace AnalyticsService.Controllers;
using Microsoft.AspNetCore.Mvc ;  
using AnalyticsService.Services.Interfaces;
using AnalyticsService.Models; 
[ApiController]
[Route("api/[controller]")]
public class StatEtdController : ControllerBase
{
    private readonly IStatistiqueUserService<StatistiqueEtudiant> _service;
    public StatEtdController(IStatistiqueUserService<StatistiqueEtudiant> svc)
        => _service = svc;
   
    
    [HttpGet("{id}/RefreshMarksStats")]
    public async Task<ActionResult<StatistiqueEtudiant>> RefreshMarksStats(int id)
    {
        var stats = await _service.CalculateStats(id); 
        if (stats == null)
        {
            // Si les statistiques ne sont pas trouvées, retourne 404
            return NotFound(new { message = $"Etudiant avec id {id} non trouvé." });
        }
        return Ok(stats); 
    } 
    [HttpPost("Create/{EtdId}")]
    public async Task<IActionResult> Create(int EtdId)
    {
        var stat = await _service.CreateAsync(EtdId);
        return Ok(stat); 
    } 
    [HttpGet("{etdId}")]
    public async Task<IActionResult> GetByEtudiant(int etdId)
    {
        try
        {
            var stats = await _service.GetByUserIdAsync(etdId);
            return Ok(stats);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { Message = $"Aucune statistique trouvée pour l'etudiant {etdId}." });
        }
    }
}
namespace AnalyticsService.Controllers;
using Microsoft.AspNetCore.Mvc ;  
using AnalyticsService.Services.Interfaces;
using AnalyticsService.Models; 
[ApiController]
[Route("api/[controller]")]
public class StatEtdController : ControllerBase
{
    private readonly IStatistiqueService<StatistiqueEtudiant> _service;
    public StatEtdController(IStatistiqueService<StatistiqueEtudiant> svc)
        => _service = svc;
   
    
    [HttpGet("{id}/Refresh")]
    public async Task<ActionResult<StatistiqueEtudiant>> GetStats(int id)
    {
        var stats = await _service.CalculateStats(id); 
        if (stats == null)
        {
            // Si les statistiques ne sont pas trouvées, retourne 404
            return NotFound(new { message = $"Etudiant avec id {id} non trouvé." });
        }
        return Ok(stats); 
    }
}
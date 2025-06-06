namespace AnalyticsService.Controllers; 
using Microsoft.AspNetCore.Mvc ;  
using AnalyticsService.Services.Interfaces;
using AnalyticsService.Models;
[ApiController]
[Route("api/analytics/modules")]
public class StatModuleController : ControllerBase
{
     private readonly IStatistiqueService<StatistiqueModule> _service;
    
            public StatModuleController(IStatistiqueService<StatistiqueModule> service)
            {
                _service = service; 
            }
    
            [HttpGet("{id}/stats")]
            public async Task<ActionResult<StatistiqueModule>> GetStats(int id)
            {
                var stats = await _service.CalculateStats(id); 
                if (stats == null)
                {
                    // Si les statistiques ne sont pas trouvées, retourne 404
                    return NotFound(new { message = $"Module avec id {id} non trouvé." });
                }
                return Ok(stats);
            }
}
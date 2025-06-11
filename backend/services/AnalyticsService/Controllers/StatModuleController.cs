namespace AnalyticsService.Controllers; 
using Microsoft.AspNetCore.Mvc ;  
using AnalyticsService.Services.Interfaces;
using AnalyticsService.Models;
[ApiController]
[Route("api/[controller]")]
public class StatModuleController : ControllerBase
{
     private readonly IStatistiqueModuleService<StatistiqueModule> _service;
    
            public StatModuleController(IStatistiqueModuleService<StatistiqueModule> service)
            {
                _service = service; 
            }
    
            [HttpGet("{id}/stats")]
            public async Task<ActionResult<StatistiqueModule>> RefreshStandardStats(int id)
            {
                var stats = await _service.CalculateStandardStats(id); 
                if (stats == null)
                {
                    // Si les statistiques ne sont pas trouvées, retourne 404
                    return NotFound(new { message = $"Module avec id {id} non trouvé." });
                }
                return Ok(stats); 
                
            }

            [HttpGet("{id}/MarksStats")]
            public async Task<ActionResult<StatistiqueModule>> RefreshMarksStats(int id)
            {
                var stats = await _service.CalculateMarksStats(id);
                if (stats == null)
                {
                    // Si les statistiques ne sont pas trouvées, retourne 404
                    return NotFound(new { message = $"Module avec id {id} non trouvé." });
                }

                return Ok(stats);
            }

            [HttpPost("Create/{ModuleId}")]
            public async Task<IActionResult> Create(int ModuleId)
            {
                var stat = await _service.CreateAsync(ModuleId);
                return Ok(stat); 
            }  
            [HttpGet("{id}/RatingStats")]
            public async Task<ActionResult<StatistiqueModule>> RefreshRatingStats(int id)
            {
                var stats = await _service.CalculateAndStoreAverageRatingAsync(id); 
                if (stats == null)
                {
                    // Si les statistiques ne sont pas trouvées, retourne 404
                    return NotFound(new { message = $"Module avec id {id} non trouvé." });
                }
                return Ok(stats);
            } 
            
       
            [HttpGet("{ModuleId}")] 
            public async Task<IActionResult> GetByModule(int ModuleId)
            {
                try
                {
                    var stats = await _service.GetByPropertyAsync(ModuleId);
                    return Ok(stats);
                }
                catch (KeyNotFoundException)
                {
                    return NotFound(new { Message = $"Aucune statistique trouvée pour le module {ModuleId}." });
                }
            }
}
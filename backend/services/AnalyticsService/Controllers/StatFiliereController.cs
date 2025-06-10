namespace AnalyticsService.Controllers;
using Microsoft.AspNetCore.Mvc ;  
using AnalyticsService.Services.Interfaces;
using AnalyticsService.Models;
[ApiController]
[Route("api/[controller]")]
public class StatFiliereController:ControllerBase
{
    private readonly IStatistiqueService<StatistiqueFiliere> _service;
    public StatFiliereController(IStatistiqueService<StatistiqueFiliere> svc)
        => _service = svc;

    [HttpGet("{id}/StandardStats")] 
    public Task<StatistiqueFiliere> RefreshStandardStats(int id)
        => _service.CalculateStandardStats(id);   
   
    [HttpPost("Create/{FiliereId:int}/{FiliereName:string}")]
    public async Task<IActionResult> Create(int FiliereId, string FiliereName)
    {
        var stat = await _service.CreateAsync(FiliereId,FiliereName);
        return Ok(stat); 
    } 
    [HttpGet("{FiliereId}")]
    public async Task<IActionResult> GetByFiliere(int FiliereId)
    {
        try
        {
            var stats = await _service.GetByPropertyAsync(FiliereId);
            return Ok(stats);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { Message = $"Aucune statistique trouvée pour la filiere {FiliereId}." });
        }
    } 
    [HttpGet("{id}/RatingStats")]
    public async Task<ActionResult<StatistiqueFiliere>> RefreshRatingStats(int id)
    {
        var stats = await _service.CalculateAndStoreAverageRatingAsync(id); 
        if (stats == null)
        {
            // Si les statistiques ne sont pas trouvées, retourne 404
            return NotFound(new { message = $"Filiere avec id {id} non trouvé." });
        }
        return Ok(stats);
    }  
    [HttpGet("{FiliereName}/MarksStats")]
    public async Task<ActionResult<StatistiqueModule>> RefreshMarksStats(String FiliereName)
    {
        var stats = await _service.CalculateMarksStats(FiliereName); 
        if (stats == null)
        {
            // Si les statistiques ne sont pas trouvées, retourne 404
            return NotFound(new { message = $"Filiere avec {FiliereName} non trouvé." });
        }
        return Ok(stats);
    } 
}
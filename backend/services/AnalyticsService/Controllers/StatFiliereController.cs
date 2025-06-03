namespace AnalyticsService.Controllers;
using Microsoft.AspNetCore.Mvc ;  
using AnalyticsService.Services.Interfaces;
using AnalyticsService.Models;
[ApiController]
[Route("api/analytics/filieres")]
public class StatFiliereController:ControllerBase
{
    private readonly IStatistiqueService<StatistiqueModule> _service;
    public StatFiliereController(IStatistiqueService<StatistiqueModule> svc)
        => _service = svc;

    [HttpGet("{id}/stats")]
    public Task<StatistiqueModule> GetStats(int id)
        => _service.CalculateStats(id); 
}
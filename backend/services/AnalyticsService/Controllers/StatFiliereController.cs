namespace AnalyticsService.Controllers;
using Microsoft.AspNetCore.Mvc ;  
using AnalyticsService.Services.Interfaces;
using AnalyticsService.Models;
[ApiController]
[Route("api/analytics/filieres")]
public class StatFiliereController:ControllerBase
{
    private readonly IStatistiqueService<StatistiqueFiliere> _service;
    public StatFiliereController(IStatistiqueService<StatistiqueFiliere> svc)
        => _service = svc;

    [HttpGet("{id}/stats")] 
    public Task<StatistiqueFiliere> GetStats(int id)
        => _service.CalculateStats(id);  
}
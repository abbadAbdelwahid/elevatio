using AnalyticsService.Services.Interfaces;

namespace AnalyticsService.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;  
[Route("api/[controller]")]
[ApiController]
public class EtdReportController : ControllerBase
{
    private readonly IReportUserService _reportUserService;

    public EtdReportController(IReportUserService reportUserService)
    {
        _reportUserService = reportUserService; 

    }  
    
}
namespace CourseManagementService.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/external-validation")]
public class ExternalValidationController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    
    public ExternalValidationController(ApplicationDbContext authDbContext) => _dbContext = authDbContext;

    [HttpPost("/api/external-validation/filieres-ids-exist")]
    public async Task<ActionResult<Dictionary<string, bool>>> FiliereIdsExists([FromBody] List<int> filieresIds)
    {
        if (filieresIds is null || !filieresIds.Any())
        {
            return BadRequest("No Filiere ids provided");
        }
        
        var distinctFilieresIds = filieresIds.Distinct();
        
        var existingUsersIds = await _dbContext.Filieres.AsNoTracking()
            .Where(f => distinctFilieresIds.Contains(f.FiliereId))
            .Select(f => f.FiliereId)
            .Distinct()
            .ToListAsync();

        var result = distinctFilieresIds.ToDictionary(id => id, id => existingUsersIds.Contains(id));
        return Ok(result);
    }

    [HttpPost("/api/external-validation/modules-ids-exist")]
    public async Task<ActionResult<Dictionary<string, bool>>> ModulesIdsExists([FromBody] List<int> modulesIds)
    {
        if (modulesIds is null || !modulesIds.Any())
        {
            return BadRequest("No modules ids provided");
        }
        
        var distinctModulesIds = modulesIds.Distinct();
        
        var existingModulesIds = await _dbContext.Modules.AsNoTracking()
            .Where(m => distinctModulesIds.Contains(m.ModuleId))
            .Select(m => m.ModuleId)
            .Distinct()
            .ToListAsync();
        
        var result = distinctModulesIds.ToDictionary(moduleId => moduleId, moduleId => existingModulesIds.Contains(moduleId));
        return Ok(result);
    }
}
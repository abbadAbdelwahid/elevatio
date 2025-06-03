using AuthService.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Controllers;

[ApiController]
[Route("api/external-validation")]
public class ExternalValidationController : ControllerBase
{
    private readonly AuthDbContext _authDbContext;
    
    public ExternalValidationController(AuthDbContext authDbContext) => _authDbContext = authDbContext;

    [HttpPost("users-ids-exist")]
    public async Task<ActionResult<Dictionary<string, bool>>> UsersIdsExists([FromBody] List<string> usersIds)
    {
        if (usersIds is null || !usersIds.Any())
        {
            return BadRequest("No users ids provided");
        }
        
        var distinctUsersIds = usersIds.Distinct();
        
        var existingUsersIds = await _authDbContext.Users.AsNoTracking()
            .Where(u => distinctUsersIds.Contains(u.Id))
            .Select(u => u.Id)
            .Distinct()
            .ToListAsync();

        var result = distinctUsersIds.ToDictionary(id => id, id => existingUsersIds.Contains(id));
        return Ok(result);
    }
}
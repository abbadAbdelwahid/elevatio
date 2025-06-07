// Controllers/EnseignantsController.cs

using System.Security.Claims;
using AuthService.Dtos;
using AuthService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace AuthService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnseignantsController : ControllerBase
{
    private readonly IEnseignantService _svc;

    public EnseignantsController(IEnseignantService svc) => _svc = svc;

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateEnseignantDto dto)
    {
        var (ok, errors, ens) = await _svc.CreateAsync(dto);
        return ok ? Ok(new { ens!.Id, ens.Email }) : BadRequest(errors);
    }

    [HttpGet("/test")]
    [Authorize]
    public OkObjectResult GetJwtInfos()
    {
        Request.Headers.TryGetValue("Authorization", out var authHeader);
        var token = authHeader.ToString().Replace("Bearer ", "");
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);
        return Ok(new {
            Header  = jwt.Header,
            Payload = jwt.Payload,
            Claims  = jwt.Claims.Select(c => new { c.Type, c.Value })
        });
    }

    [Authorize(Roles = "Admin,Enseignant")]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        
        var ens = await _svc.GetByIdAsync(id);
        return ens is null ? NotFound() : Ok(ens);
        
    }
    
    // Controllers/EnseignantsController.cs
    [Authorize(Roles = "Admin,Enseignant")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, UpdateEnseignantDto dto)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Récupérer l'ID de l'utilisateur authentifié
        var success = await _svc.UpdateAsync(id, dto, currentUserId);
        return success ? NoContent() : Forbid();  // Si l'ID ne correspond pas, renvoie 403 Forbidden
    }


    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var success = await _svc.DeleteAsync(id);
        return success ? NoContent() : NotFound();
    }

}
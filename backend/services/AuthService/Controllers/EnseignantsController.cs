// Controllers/EnseignantsController.cs

using System.Security.Claims;
using AuthService.Dtos;
using AuthService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
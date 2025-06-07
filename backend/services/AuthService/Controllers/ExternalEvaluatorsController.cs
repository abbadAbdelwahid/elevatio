// Controllers/ExternalEvaluatorsController.cs

using System.Security.Claims;
using AuthService.Dtos;
using AuthService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExternalEvaluatorsController : ControllerBase
{
    private readonly IExternalEvaluatorService _svc;
    public ExternalEvaluatorsController(IExternalEvaluatorService svc) => _svc = svc;

    // [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateExternalEvaluatorDto dto)
    {
        var (ok, errors, ext) = await _svc.CreateAsync(dto);
        return ok ? Ok(new { ext!.Id, ext.Email }) : BadRequest(errors);
    }

    // [Authorize(Roles = "Admin,ExternalEvaluator")]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var ext = await _svc.GetByIdAsync(id);
        return ext is null ? NotFound() : Ok(ext);
    }
    
    // [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? domaine = null)
    {
        var list = await _svc.GetAllAsync(domaine);
        return Ok(list);
    }

    // [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, UpdateExternalEvaluatorDto dto)
    {
        var success = await _svc.UpdateAsync(id, dto);
        return success ? NoContent() : NotFound();
    }

    // [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var success = await _svc.DeleteAsync(id);
        return success ? NoContent() : NotFound();
    }
    
    // [Authorize(Roles = "Admin,ExternalEvaluator")]
    [HttpGet("me")] // Route pour obtenir les informations de l'évaluateur externe connecté
    public async Task<IActionResult> GetMe()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);  // Récupérer l'ID de l'utilisateur connecté

        var evaluator = await _svc.GetByIdAsync(userId);  // Utiliser l'ID pour obtenir les informations de l'évaluateur externe

        return evaluator is null ? NotFound() : Ok(evaluator);  // Retourner les données ou 404 si l'évaluateur n'existe pas
    }


    
}
// Controllers/EtudiantsController.cs
using AuthService.Dtos;
using AuthService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EtudiantsController : ControllerBase
{
    private readonly IEtudiantService _svc;
    public EtudiantsController(IEtudiantService svc) => _svc = svc;

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateEtudiantDto dto)
    {
        var (ok, errors, etu) = await _svc.CreateAsync(dto);
        return ok ? Ok(new { etu!.Id, etu.Email }) : BadRequest(errors);
    }

    [Authorize(Roles = "Admin,Etudiant")]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var etu = await _svc.GetByIdAsync(id);
        return etu is null ? NotFound() : Ok(etu);
    }
}
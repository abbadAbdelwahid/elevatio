// Controllers/EnseignantsController.cs

using System.Security.Claims;
using AuthService.Data;
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
    private readonly AuthDbContext _db;


    public EnseignantsController(IEnseignantService svc,AuthDbContext db)
    {
        _svc = svc;
        _db = db; // Injection de AuthDbContext

    }

    // [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateEnseignantDto dto)
    {
        var (ok, errors, ens) = await _svc.CreateAsync(dto);
        return ok ? Ok(new { ens!.Id, ens.Email }) : BadRequest(errors);
    }

    // [Authorize(Roles = "Admin,Enseignant")]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var ens = await _svc.GetByIdAsync(id);
        return ens is null ? NotFound() : Ok(ens);
        
    }
    
    // Controllers/EnseignantsController.cs
    // [Authorize(Roles = "Admin,Enseignant")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, UpdateEnseignantDto dto)
    {
        var success = await _svc.UpdateAsync(id, dto);
        return success ? NoContent() : NotFound("Enseignant introuvable.");
    }



    // [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var success = await _svc.DeleteAsync(id);
        return success ? NoContent() : NotFound();
    }
    
    // [Authorize(Roles = "Enseignant")]
    [HttpGet("me")] // Route pour obtenir les informations de l'enseignant connecté
    public async Task<IActionResult> GetMe()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);  // Récupérer l'ID de l'utilisateur connecté

        var enseignant = await _svc.GetByIdAsync(userId);  // Utiliser l'ID pour obtenir les informations de l'enseignant

        return enseignant is null ? NotFound() : Ok(enseignant);  // Retourner les données ou 404 si l'enseignant n'existe pas
    }

    // [Authorize(Roles = "Enseignant")]
    [HttpPut("me/profile-image")]
    public async Task<IActionResult> UpdateProfileImage(IFormFile file)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Récupérer l'ID de l'utilisateur connecté

        var enseignant = await _svc.GetByIdAsync(userId);  // Récupérer l'enseignant connecté

        if (enseignant == null)
            return NotFound("Enseignant non trouvé.");

        try
        {
            // Télécharger et sauvegarder l'image
            var imageUrl = await _svc.UploadProfileImage(file);

            // Mettre à jour l'URL de l'image de profil dans l'entité enseignant
            enseignant.ProfileImageUrl = imageUrl;

            // Utiliser Update() pour s'assurer que l'objet est bien mis à jour dans le contexte
            _db.Enseignants.Update(enseignant);  // Mise à jour de l'enseignant dans la base de données

            // Sauvegarder les changements dans la base de données
            await _db.SaveChangesAsync();  // Appliquer les changements

            return Ok(new { imageUrl = imageUrl }); // Retourner l'URL de l'image
        }
        catch (Exception ex)
        {
            return BadRequest($"Erreur de téléchargement de l'image : {ex.Message}");
        }
        
    }

    
    [HttpGet("{id}/fullname")]
    public async Task<IActionResult> GetFullName(string id)
    {
        var fullName = await _svc.GetFullNameByIdAsync(id);

        if (fullName == null)
            return NotFound("Enseignant introuvable.");

        return Ok(new { fullName });
    }
    
    [HttpPut("{id}/profile-image")]
    public async Task<IActionResult> UpdateProfileImageById(string id, IFormFile file)
    {
        var enseignant = await _svc.GetByIdAsync(id);
        if (enseignant is null)
            return NotFound("Enseignant introuvable.");

        try
        {
            var imageUrl = await _svc.UploadProfileImage(file);

            enseignant.ProfileImageUrl = imageUrl;
            _db.Enseignants.Update(enseignant);
            await _db.SaveChangesAsync();

            return Ok(new { imageUrl });
        }
        catch (Exception ex)
        {
            return BadRequest($"Erreur de téléchargement de l'image : {ex.Message}");
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var enseignants = await _svc.GetAllAsync();
        return Ok(enseignants);
    }


}
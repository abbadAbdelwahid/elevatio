// Controllers/EtudiantsController.cs

using System.Security.Claims;
using AuthService.Data;
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
    private readonly AuthDbContext _db;
    
    public EtudiantsController(IEtudiantService svc, AuthDbContext db){
        _svc = svc;
        _db = db; // Injection de AuthDbContext
    }

    // [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateEtudiantDto dto)
    {
        var (ok, errors, etu) = await _svc.CreateAsync(dto);
        return ok ? Ok(new { etu!.Id, etu.Email }) : BadRequest(errors);
    }

    // [Authorize(Roles = "Admin,Etudiant")]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var etu = await _svc.GetByIdAsync(id);
        return etu is null ? NotFound() : Ok(etu);
    }
    
    // [Authorize(Roles = "Admin,Etudiant")]
    [HttpGet("me")] // Route pour obtenir les informations de l'étudiant connecté
    public async Task<IActionResult> GetMe()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);  // Récupérer l'ID de l'utilisateur connecté

        var etudiant = await _svc.GetByIdAsync(userId);  // Utiliser l'ID pour obtenir les informations de l'étudiant

        return etudiant is null ? NotFound() : Ok(etudiant);  // Retourner les données ou 404 si l'étudiant n'existe pas
    }

    // [Authorize(Roles = "Admin,Etudiant")]
    [HttpPut("me/profile-image")]
    public async Task<IActionResult> UpdateProfileImage(IFormFile file)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Récupérer l'ID de l'utilisateur connecté

        var etudiant = await _svc.GetByIdAsync(userId);  // Récupérer l'étudiant connecté

        if (etudiant == null)
            return NotFound("Étudiant non trouvé.");

        try
        {
            // Télécharger et sauvegarder l'image
            var imageUrl = await _svc.UploadProfileImage(file);

            // Vérifier si l'URL de l'image de profil est bien mise à jour
            etudiant.ProfileImageUrl = imageUrl;

            // Sauvegarder l'URL de l'image dans la base de données
            _db.Etudiants.Update(etudiant);  // Assurez-vous d'utiliser Update() si nécessaire
            await _db.SaveChangesAsync();  // Appliquer les modifications dans la base de données

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
            return NotFound("Étudiant introuvable.");

        return Ok(new { fullName });
    }
    
    [HttpPut("{id}/profile-image")]
    public async Task<IActionResult> UpdateProfileImageById(string id, IFormFile file)
    {
        var etudiant = await _svc.GetByIdAsync(id);  // Cherche l'étudiant par ID

        if (etudiant == null)
            return NotFound("Étudiant non trouvé.");

        try
        {
            // Upload de l’image
            var imageUrl = await _svc.UploadProfileImage(file);

            // Mise à jour de l’URL
            etudiant.ProfileImageUrl = imageUrl;

            // Sauvegarde dans la BDD
            _db.Etudiants.Update(etudiant);
            await _db.SaveChangesAsync();

            return Ok(new { imageUrl });
        }
        catch (Exception ex)
        {
            return BadRequest($"Erreur lors de l'envoi de l'image : {ex.Message}");
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var etudiants = await _svc.GetAllAsync();
        return Ok(etudiants);
    }

    [HttpGet("filiere/{filiereId}")]
    public async Task<IActionResult> GetByFiliereId(int filiereId)
    {
        var etudiants = await _svc.GetByFiliereIdAsync(filiereId);
        if (etudiants == null || !etudiants.Any())
            return NotFound("Aucun étudiant trouvé pour cette filière.");

        return Ok(etudiants);
    }

    
}
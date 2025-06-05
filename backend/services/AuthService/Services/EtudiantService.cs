// Services/EtudiantService.cs
using AuthService.Data;
using AuthService.Dtos;
using AuthService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Services;

public class EtudiantService : IEtudiantService
{
    private readonly UserManager<ApplicationUser> _userMgr;
    private readonly AuthDbContext _db;

    public EtudiantService(UserManager<ApplicationUser> userMgr, AuthDbContext db)
    {
        _userMgr = userMgr;
        _db = db;
    }

    public async Task<(bool Ok, IEnumerable<string> Errors, Etudiant? Data)> CreateAsync(CreateEtudiantDto dto)
    {
        var etu = new Etudiant
        {
            UserName  = dto.Email,
            Email     = dto.Email,
            FirstName = dto.FirstName,
            LastName  = dto.LastName,
            FiliereId = dto.FiliereId
        };

        var res = await _userMgr.CreateAsync(etu, dto.Password);
        if (!res.Succeeded)
            return (false, res.Errors.Select(e => e.Description), null);

        await _userMgr.AddToRoleAsync(etu, "Etudiant");
        return (true, Enumerable.Empty<string>(), etu);
    }

    public async Task<Etudiant?> GetByIdAsync(string id) =>
        await _db.Etudiants.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
    
    // Services/EtudiantService.cs
    public async Task<string> UploadProfileImage(IFormFile file)
    {
        // Vérifier si le fichier est nul ou a une taille invalide
        if (file == null || file.Length == 0)
            throw new Exception("Aucune image n'a été téléchargée.");

        // Générer un nom de fichier unique
        var fileName = Path.GetFileName(Guid.NewGuid().ToString() + Path.GetExtension(file.FileName));
        var filePath = Path.Combine("wwwroot", "uploads", "profile_images", fileName);

        // Créer le dossier si nécessaire
        var directoryPath = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        // Sauvegarder l'image
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return $"/uploads/profile_images/{fileName}"; // Retourner l'URL de l'image
    }

    
}
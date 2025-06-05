// Services/EnseignantService.cs
using AuthService.Data;
using AuthService.Dtos;
using AuthService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Services;

public class EnseignantService : IEnseignantService
{
    private readonly UserManager<ApplicationUser> _userMgr;
    private readonly AuthDbContext _db;

    public EnseignantService(UserManager<ApplicationUser> userMgr, AuthDbContext db)
    {
        _userMgr = userMgr;
        _db = db;
    }

    public async Task<(bool Ok, IEnumerable<string> Errors, Enseignant? Data)> CreateAsync(CreateEnseignantDto dto)
    {
        var ens = new Enseignant
        {
            UserName     = dto.Email,
            Email        = dto.Email,
            FirstName    = dto.FirstName,
            LastName     = dto.LastName,
            Grade        = dto.Grade,
            Specialite   = dto.Specialite,
            DateEmbauche = dto.DateEmbauche
        };

        var res = await _userMgr.CreateAsync(ens, dto.Password);
        if (!res.Succeeded)
            return (false, res.Errors.Select(e => e.Description), null);

        await _userMgr.AddToRoleAsync(ens, "Enseignant");
        return (true, Enumerable.Empty<string>(), ens);
    }

    public async Task<Enseignant?> GetByIdAsync(string id) =>
        await _db.Enseignants.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
    
    public async Task<bool> UpdateAsync(string id, UpdateEnseignantDto dto, string currentUserId)
    {
        // Vérifier que l'ID de l'enseignant à mettre à jour correspond à l'utilisateur authentifié
        if (id != currentUserId) return false;

        var enseignant = await _db.Enseignants.FindAsync(id);
        if (enseignant is null) return false;

        enseignant.FirstName = dto.FirstName;
        enseignant.LastName = dto.LastName;
        enseignant.Grade = dto.Grade;
        enseignant.Specialite = dto.Specialite;

        _db.Enseignants.Update(enseignant);
        await _db.SaveChangesAsync();
        return true;
    }


    public async Task<bool> DeleteAsync(string id)
    {
        var enseignant = await _db.Enseignants.FindAsync(id);
        if (enseignant is null) return false;

        _db.Enseignants.Remove(enseignant);
        await _db.SaveChangesAsync();
        return true;
    }
    
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
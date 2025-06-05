// Models/Etudiant.cs
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthService.Models;

public class Etudiant : ApplicationUser
{
    public string FiliereId { get; set; } = default!;
    public string? ProfileImageUrl { get; set; } // Nouveau champ pour stocker l'URL de l'image de profil

}
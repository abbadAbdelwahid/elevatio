// Models/Etudiant.cs
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthService.Models;

public class Etudiant : ApplicationUser
{
    public int FiliereId { get; set; }
    public string? ProfileImageUrl { get; set; } // Nouveau champ pour stocker l'URL de l'image de profil

}
// Models/Enseignant.cs
namespace AuthService.Models;

public class Enseignant : ApplicationUser
{
    public string? Grade          { get; set; }
    public string? Specialite     { get; set; }
    public DateTime? DateEmbauche { get; set; }
    
    public string? ProfileImageUrl { get; set; }  // Ajout du champ pour l'URL de l'image de profil

}
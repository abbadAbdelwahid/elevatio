// Models/Enseignant.cs
namespace AuthService.Models;

public class Enseignant : ApplicationUser
{
    public string? Grade          { get; set; }
    public string? Specialite     { get; set; }
    public DateTime? DateEmbauche { get; set; }
}
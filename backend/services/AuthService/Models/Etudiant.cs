// Models/Etudiant.cs
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthService.Models;

public class Etudiant : ApplicationUser
{
    public string FiliereId { get; set; } = default!;
}
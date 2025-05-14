// Models/ExternalEvaluator.cs
namespace AuthService.Models;

public class ExternalEvaluator : ApplicationUser
{
    public string Organisation { get; set; } = default!;
    public string Domaine { get; set; } = default!;
}
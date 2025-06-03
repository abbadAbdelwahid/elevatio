// SeedData/RoleSeeder.cs
using Microsoft.AspNetCore.Identity;

namespace AuthService.SeedData;

public static class RoleSeeder
{
    public static async Task SeedRolesAsync(IServiceProvider sp)
    {
        var roleMgr = sp.GetRequiredService<RoleManager<IdentityRole>>();
        string[] roles = { "Admin", "Enseignant", "Etudiant","ExternalEvaluator" };

        foreach (var r in roles)
            if (!await roleMgr.RoleExistsAsync(r))
                await roleMgr.CreateAsync(new IdentityRole(r));
    }
}
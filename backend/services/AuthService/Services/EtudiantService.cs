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
}
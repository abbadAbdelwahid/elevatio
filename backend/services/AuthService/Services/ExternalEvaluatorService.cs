// Services/ExternalEvaluatorService.cs
using AuthService.Data;
using AuthService.Dtos;
using AuthService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Services;

public class ExternalEvaluatorService : IExternalEvaluatorService
{
    private readonly UserManager<ApplicationUser> _userMgr;
    private readonly AuthDbContext _db;

    public ExternalEvaluatorService(UserManager<ApplicationUser> userMgr, AuthDbContext db)
    {
        _userMgr = userMgr;
        _db = db;
    }

    public async Task<(bool Ok, IEnumerable<string> Errors, ExternalEvaluator? Data)> CreateAsync(CreateExternalEvaluatorDto dto)
    {
        var ext = new ExternalEvaluator
        {
            UserName     = dto.Email,
            Email        = dto.Email,
            FirstName    = dto.FirstName,
            LastName     = dto.LastName,
            Organisation = dto.Organisation,
            Domaine      = dto.Domaine
        };

        var res = await _userMgr.CreateAsync(ext, dto.Password);
        if (!res.Succeeded)
            return (false, res.Errors.Select(e => e.Description), null);

        await _userMgr.AddToRoleAsync(ext, "ExternalEvaluator");
        return (true, Enumerable.Empty<string>(), ext);
    }

    public async Task<ExternalEvaluator?> GetByIdAsync(string id) =>
        await _db.ExternalEvaluators.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
    
    public async Task<IEnumerable<ExternalEvaluator>> GetAllAsync(string? domaine = null)
    {
        var query = _db.ExternalEvaluators.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(domaine))
            query = query.Where(e => e.Domaine.ToLower().Contains(domaine.ToLower()));

        return await query.ToListAsync();
    }

    public async Task<bool> UpdateAsync(string id, UpdateExternalEvaluatorDto dto)
    {
        var eval = await _db.ExternalEvaluators.FindAsync(id);
        if (eval is null) return false;

        eval.FirstName = dto.FirstName;
        eval.LastName = dto.LastName;
        eval.Organisation = dto.Organisation;
        eval.Domaine = dto.Domaine;

        _db.ExternalEvaluators.Update(eval);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var eval = await _db.ExternalEvaluators.FindAsync(id);
        if (eval is null) return false;

        _db.ExternalEvaluators.Remove(eval);
        await _db.SaveChangesAsync();
        return true;
    }

}
using EvaluationService.Data;
using EvaluationService.Models;
using Microsoft.EntityFrameworkCore;

namespace EvaluationService.Repositories;

public class EvaluationRepository : IEvaluationRepository
{
    private readonly EvaluationsDbContext _ctx;
    public EvaluationRepository(EvaluationsDbContext ctx) => _ctx = ctx;

    public async Task<List<Evaluation>> AddRangeAsync(List<Evaluation> evaluations)
    {
        _ctx.Evaluations.AddRange(evaluations);
        await _ctx.SaveChangesAsync();
        return evaluations;
    }

    public string GetEvaluationType(int evaluationId)
    {
        if(_ctx.Evaluations.FindAsync(evaluationId).Result?.FiliereId != null) return "Filiere";
        else if (_ctx.Evaluations.FindAsync(evaluationId).Result?.ModuleId != null) return "Module";
        return "Invalid_Evaluation_Type";
    }
    
    public async Task<List<Evaluation>> GetEvaluationsByFiliereIdAsync(int filiereId)
    {
        return await _ctx.Evaluations.AsNoTracking()
            .Where(e => e.FiliereId == filiereId)
            .ToListAsync<Evaluation>();
    }

    public async Task<List<Evaluation>> GetEvaluationsByModuleIdAsync(int moduleId)
    {
        return await _ctx.Evaluations.AsNoTracking()
            .Where(e => e.ModuleId == moduleId)
            .ToListAsync();
    }

    public async Task<List<Evaluation>> GetEvaluationsByRespondentIdAsync(string respondentId)
    {
        return await _ctx.Evaluations.AsNoTracking()
            .Where(e=> e.RespondentUserId != null &&
                       e.RespondentUserId.Equals(respondentId, StringComparison.CurrentCultureIgnoreCase))
            .ToListAsync<Evaluation>();    
    }

    public async Task<List<Evaluation>> GetAllEvaluationsAsync()
    {
        return await _ctx.Evaluations.AsNoTracking().ToListAsync();
    }

    public async Task<Evaluation> GetEvaluationByIdAsync(int evaluationId)
    {
        return await _ctx.Evaluations.FindAsync(evaluationId)
               ?? throw new KeyNotFoundException();
    }

    public async Task<Evaluation> DeleteEvaluationByIdAsync(int evaluationId)
    {
        var evaluation = GetEvaluationByIdAsync(evaluationId).Result;
        _ctx.Evaluations.Remove(evaluation);
        await _ctx.SaveChangesAsync();
        return evaluation;
    }

    public async Task<List<Evaluation>> DeleteEvaluationsByRespondentIdAsync(string respondentId)
    {
        var evaluationsOfRespondent = await GetEvaluationsByRespondentIdAsync(respondentId);
        _ctx.Evaluations.RemoveRange(evaluationsOfRespondent);
        await _ctx.SaveChangesAsync();
        return evaluationsOfRespondent;
    }

    public async Task<List<Evaluation>> DeleteEvaluationsByFiliereIdAsync(int filiereId)
    {
       var evaluationsOfFiliere = await GetEvaluationsByFiliereIdAsync(filiereId);
       _ctx.Evaluations.RemoveRange(evaluationsOfFiliere);
       await _ctx.SaveChangesAsync();
       return evaluationsOfFiliere;
    }

    public async Task<List<Evaluation>> DeleteEvaluationsByModuleIdAsync(int evaluationId)
    {
        List<Evaluation> evaluations = await GetEvaluationsByModuleIdAsync(evaluationId);
        _ctx.Evaluations.RemoveRange(evaluations);
        await _ctx.SaveChangesAsync();
        return evaluations;
    }

    public async Task<Evaluation> UpdateEvaluationAsync(Evaluation evaluation)
    {
        _ctx.Evaluations.Update(evaluation);
        await _ctx.SaveChangesAsync();
        return evaluation;
        
    }

    public async Task<Evaluation> AddEvaluationAsync(Evaluation evaluation)
    {
        await _ctx.Evaluations.AddAsync(evaluation);
        await _ctx.SaveChangesAsync();
        return evaluation;
    }

    public async Task<List<Evaluation>> DeleteRangeAsync(List<Evaluation> evaluations)
    {
        _ctx.Evaluations.RemoveRange(evaluations);
        await _ctx.SaveChangesAsync();
        return evaluations;   
    }
}
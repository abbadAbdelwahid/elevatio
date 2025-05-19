using EvaluationService.Data;
using EvaluationService.Models;
using Microsoft.EntityFrameworkCore;

namespace EvaluationService.Repositories;

public class QuestionsStandardRepository : IQuestionsStandardRepository
{
    private readonly EvaluationsDbContext _ctx;
    
    public QuestionsStandardRepository(EvaluationsDbContext ctx) => _ctx = ctx;
    
    public async Task<List<StandardQuestion>> GetQuestionsStandardsByStatName(StatName statName)
    {
        return await _ctx.StandardQuestions.AsNoTracking()
            .Where(q => q.StatName == statName)
            .ToListAsync();
    }

    public async Task<StandardQuestion> GetStandardQuestionById(int id)
    {
        return await _ctx.StandardQuestions.AsNoTracking()
            .FirstOrDefaultAsync(q => q.StandardQuestionId == id)
            ?? throw new NullReferenceException("StandardQuestion not found");
    }

    public async Task<List<StandardQuestion>> GetStandardQuestions()
    {
        return await _ctx.StandardQuestions.AsNoTracking().ToListAsync();
    }

    public async Task<StandardQuestion> DeleteStandardQuestionById(int id)
    {
        var sqToRemove = await GetStandardQuestionById(id);
        _ctx.StandardQuestions.Remove(sqToRemove);
        await _ctx.SaveChangesAsync();
        return sqToRemove;
    }

    public async Task<List<StandardQuestion>> DeleteStandardQuestionsByStatName(StatName statName)
    {
        var sqList = new List<StandardQuestion>();
        foreach (var sq in await GetQuestionsStandardsByStatName(statName))
        {
            sqList.Add(sq);
            _ctx.StandardQuestions.Remove(sq);
            await _ctx.SaveChangesAsync();
        }
        return sqList;
    }

    public async Task<StandardQuestion> AddStandardQuestion(StandardQuestion standardQuestion)
    {
        await _ctx.StandardQuestions.AddAsync(standardQuestion);
        await _ctx.SaveChangesAsync();
        return standardQuestion;
    }

    public async Task<StandardQuestion> UpdateStandardQuestion(StandardQuestion standardQuestion)
    {
        _ctx.StandardQuestions.Update(standardQuestion);
        await _ctx.SaveChangesAsync();
        return standardQuestion;
    }
    
}
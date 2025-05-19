using EvaluationService.Controllers;
using EvaluationService.Data;
using EvaluationService.DTOs;
using EvaluationService.Models;
using Microsoft.EntityFrameworkCore;

namespace EvaluationService.Repositories;

public class AnswerRepository : IAnswerRepository
{
    private readonly EvaluationsDbContext _ctx;
    public AnswerRepository(EvaluationsDbContext ctx) => _ctx = ctx;

    public async Task<List<Answer>> AddRangeAsync(List<Answer> answers)
    {
        _ctx.Answers.AddRange(answers);
        await _ctx.SaveChangesAsync();
        return answers.ToList();
    }

    public async Task<(int? FiliereId, int? ModuleId)> GetQuestionnaireIdsAsync(int answerId)
    {
        var ids = await _ctx.Answers.Where(a => a.AnswerId == answerId)
            .AsNoTracking()
            .Include(a => a.Question)
            .ThenInclude(q => q!.Questionnaire)
            .Select(a => new
            {
                filiereId = a.Question!.Questionnaire!.FiliereId,
                moduleId = a.Question!.Questionnaire!.ModuleId
            }).FirstOrDefaultAsync();
        
        if(ids == null) throw new KeyNotFoundException($"Answer {answerId} not found");
        return (ids.filiereId, ids.moduleId);
    }
    

    public async Task<string> GetAnswerTypeFiliereModuleAsync(int answerId)
    {
        var ids = await GetQuestionnaireIdsAsync(answerId);
        if (ids.FiliereId != null && ids.ModuleId == null) return "Filiere";
        if (ids.FiliereId == null && ids.ModuleId != null) return "Module";
        return "Invalid_Questionnaire_Type";
    }    
    
    public async Task<List<Answer>> GetAnswersFiliereAsync()
    {
        return await _ctx.Answers
            .AsNoTracking()
            .Include(a => a.Question)
            .ThenInclude(q => q!.Questionnaire)
            .Where(a => a.Question!.Questionnaire!.FiliereId != null
                    && a.Question.Questionnaire.ModuleId == null)
            .ToListAsync();
    }    
    
    public async Task<List<Answer>> GetAnswersModuleAsync()
    {
        return await _ctx.Answers
            .AsNoTracking()
            .Include(a => a.Question)
            .ThenInclude(q => q!.Questionnaire)
            .Where(a => a.Question!.Questionnaire!.FiliereId == null
                        && a.Question.Questionnaire.ModuleId != null)
            .ToListAsync();
    }

    public Task<QuestionnaireType> GetQuestionnaireTypeInternalExternalAsync(int answerId)
    {
        return _ctx.Answers
            .AsNoTracking()
            .Where(a => a.AnswerId == answerId)
            .Include(a=> a.Question)
            .ThenInclude(q => q!.Questionnaire)
            .Select(a => a.Question!.Questionnaire!.Type)
            .FirstOrDefaultAsync();
    }
    
    public async Task<List<Answer>> GetAnswersByQuestionIdAsync(int questionId)
    {
        return await _ctx.Answers
            .AsNoTracking()
            .Include(a=> a.Question)
            .Where(a=> a.QuestionId == questionId)
            .ToListAsync<Answer>();
    }

    public async Task<List<Answer>> GetAnswersByQuestionnaireIdAsync(int questionnaireId)
    {
        return await _ctx.Answers
            .AsNoTracking()
            .Include(a=> a.Question)
            .ThenInclude(q => q!.Questionnaire)
            .Where(a=> a.Question!.Questionnaire!.QuestionnaireId == questionnaireId)
            .ToListAsync<Answer>();
    }

    public async Task<List<Answer>> GetAnswersByRespondentIdAsync(string respondentId)
    {
        return await _ctx.Answers.AsNoTracking()
            .Include(a=> a.Question)
            .ThenInclude(q => q!.Questionnaire)
            .Where(a => a.Question!.Questionnaire!.RespondentUserId != null && 
                        a.Question.Questionnaire.RespondentUserId.Equals(respondentId, StringComparison.CurrentCultureIgnoreCase))
            .ToListAsync<Answer>();
    }

    public async Task<List<Answer>> GetAllAnswersAsync()
    {
        return await _ctx.Answers.ToListAsync();
    }

    public async Task<Answer> GetAnswerByIdAsync(int answerId)
    {
        return await _ctx.Answers.FindAsync(answerId) ?? throw new KeyNotFoundException($"Answer {answerId} not found");
    }

    public async Task<Answer> DeleteAnswerByIdAsync(int answerId)
    {
        var answer = await GetAnswerByIdAsync(answerId);
        _ctx.Answers.Remove(answer);
        await _ctx.SaveChangesAsync();
        return answer;
    }

    public async Task<Answer> UpdateAnswerAsync(Answer answer)
    {
        _ctx.Answers.Update(answer);
        await _ctx.SaveChangesAsync();
        return answer;
    }

    public async Task<Answer> AddAnswerAsync(Answer answer)
    {
        _ctx.Answers.Add(answer);
        await _ctx.SaveChangesAsync();
        return answer;
    }

    public async Task<List<Answer>> DeleteRangeAsync(List<Answer> answers)
    {
        _ctx.Answers.RemoveRange(answers);
        await _ctx.SaveChangesAsync();
        return answers;   
    }

    public async Task<List<Question>> GetQuestionsOfAnswersList(List<Answer> answers)
    {
        List<Question> questions = new List<Question>();
        foreach (var answer in answers)
        {
            var question = await GetQuestionOfAnswer(answer);
            questions.Add(question);
        }
        return questions;
    }

    public async Task<Question> GetQuestionOfAnswer(Answer answer)
    {
        var question = await _ctx.Answers.AsNoTracking()
            .Where(q => q.QuestionId == answer.QuestionId)
            .Include(q => q.Question)
            .Select(q => q.Question)
            .FirstOrDefaultAsync();
        if(question == null) throw new NullReferenceException($"Question of answer {answer.AnswerId} not found");
        return question;
    }
    
    public async Task<Question> GetQuestionOfAnswerById(int answerId)
    {
        var question = await _ctx.Answers.AsNoTracking()
            .Where(a => a.AnswerId == answerId)
            .Include(q => q.Question)
            .Select(q => q.Question)
            .FirstOrDefaultAsync();
        if(question == null) throw new NullReferenceException($"Question of answer {answerId} not found");
        return question;
    }
}
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

    public async Task<TypeModuleFiliere?> GetQuestionnaireTypeFiliereModuleAsync(int answerId)
    {
        var answer = await GetAnswerByIdAsync(answerId);
        return await _ctx.Answers.Where(a => a.AnswerId == answer.AnswerId)
            .Include(a => a.Question)
            .ThenInclude(q => q!.Questionnaire)
            .Select(q => q!.Question.Questionnaire.TypeModuleFiliere)
            .FirstOrDefaultAsync();
    }

    public async Task<TypeInternalExternal?> GetQuestionnaireTypeInternalExternalAsync(int answerId)
    {
        var answer = await GetAnswerByIdAsync(answerId);
        return await _ctx.Answers
            .AsNoTracking()
            .Where(a => a.AnswerId == answer.AnswerId)
            .Include(a => a.Question)
            .ThenInclude(q => q!.Questionnaire)
            .Select(q => q!.Question.Questionnaire.TypeInternalExternal)
            .FirstOrDefaultAsync();
    }
    
    public async Task<List<Answer>> GetAnswersFiliereAsync()
    {
        return await _ctx.Answers
            .AsNoTracking()
            .Include(a => a.Question)
            .ThenInclude(q => q!.Questionnaire)
            .Where(a => a.Question!.Questionnaire!.TypeModuleFiliere == TypeModuleFiliere.Filiere)
            .ToListAsync();
    }    
    
    public async Task<List<Answer>> GetAnswersModuleAsync()
    {
        return await _ctx.Answers
            .AsNoTracking()
            .Include(a => a.Question)
            .ThenInclude(q => q!.Questionnaire)
            .Where(a => a.Question!.Questionnaire!.TypeModuleFiliere == TypeModuleFiliere.Module)
            .ToListAsync();
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
            .Where(a => a.RespondentUserId != null && 
                        a.RespondentUserId.ToLower() == respondentId.ToLower())
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

    public async Task<List<Answer>> DeleteAnswersByRespondentId(string respondentId)
    {
        var answersToDelete = await GetAnswersByRespondentIdAsync(respondentId);
        _ctx.Answers.RemoveRange(answersToDelete);
        await _ctx.SaveChangesAsync();
        return answersToDelete;
    }

    public async Task<List<Question?>> GetQuestionsOfAnswersList(List<Answer> answers)
    {
        if(answers == null || answers.Count == 0)
            return new List<Question>();

        var questionsTask = answers.Select(async a =>
            {
                try
                {
                    return await GetQuestionOfAnswerById(a.AnswerId);
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine(e);
                    return null;
                }
            }
        );
        var questions = await Task.WhenAll(questionsTask);
        return questions.Where(q => q != null).ToList();
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

    public async Task<List<string?>> GetAllRespondentsIdsAsync()
    {
        return await _ctx.Answers.AsNoTracking()
            .Where(a => a.RespondentUserId != null)
            .Select(a => a.RespondentUserId)
            .ToListAsync();
    }
}
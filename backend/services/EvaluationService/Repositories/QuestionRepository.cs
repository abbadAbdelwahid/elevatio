using EvaluationService.Data;
using EvaluationService.Models;
using Microsoft.EntityFrameworkCore;

namespace EvaluationService.Repositories;

public class QuestionRepository : IQuestionRepository
{
    private readonly EvaluationsDbContext _ctx;
    public QuestionRepository(EvaluationsDbContext ctx) => _ctx = ctx;

    public async Task<List<Question>> AddRangeAsync(List<Question> questions)
    {
        await _ctx.Questions.AddRangeAsync(questions);
        await _ctx.SaveChangesAsync();
        return questions;
    }

    public async Task<List<Question>> GetAllQuestionsAsync()
    {
        return await _ctx.Questions
            .Include(q => q.StandardQuestion)
            .ToListAsync();
    }
    
    public async Task<List<Question>> GetQuestionsByQuestionnaireIdAsync(int questionnaireId)
    {
        return await _ctx.Questions
            .Where(q => q.QuestionnaireId == questionnaireId)
            .Include(q => q.StandardQuestion)
            .ToListAsync();
    }

    public async Task<List<StandardQuestion>> GetStandardQuestionsAsync()
    {
        List<Question> standardQuestions = await _ctx.Questions
            .AsNoTracking()
            .Where(q => q.StandardQuestionId != null)
            .ToListAsync();
        
        List<StandardQuestion> standardQuestionsList = new List<StandardQuestion>();
        foreach (var sq in standardQuestions)
        {
            var standardQuestion = _ctx.StandardQuestions.AsNoTracking()
                .FirstOrDefault(q => q.StandardQuestionId == sq.StandardQuestionId);
            if(standardQuestion != null)
                standardQuestionsList.Add(standardQuestion);
        }
        return standardQuestionsList;
    }

    public async Task<Question> AddQuestionAsync(Question question)
    {
        await _ctx.Questions.AddAsync(question);
        await _ctx.SaveChangesAsync();
        return question;
    }

    public async Task<Question> GetQuestionAsync(int questionId)
    {
        return await _ctx.Questions.AsNoTracking()
                   .Include(q => q.StandardQuestion)
                   .FirstOrDefaultAsync(q => q.QuestionId == questionId)
                    ?? throw new NullReferenceException();
    }

    public async Task<Question> DeleteQuestionAsync(int questionId)
    {
        var questionToDelete = await GetQuestionAsync(questionId);
        _ctx.Questions.Remove(questionToDelete);
        await _ctx.SaveChangesAsync();
        return questionToDelete;
    }

    public async Task<Question> UpdateQuestionAsync(Question question)
    {
        _ctx.Questions.Update(question);
        await _ctx.SaveChangesAsync();
        return question;
    }

    public async Task<List<Question>> DeleteRangeAsync(List<Question> questions)
    {
        _ctx.Questions.RemoveRange(questions);
        await _ctx.SaveChangesAsync();
        return questions;
    }
}
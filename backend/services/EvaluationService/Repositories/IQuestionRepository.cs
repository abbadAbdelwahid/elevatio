using EvaluationService.Models;

namespace EvaluationService.Repositories;

public interface IQuestionRepository
{
    Task<List<Question>> AddRangeAsync(List<Question> questions);
    Task<List<Question>> GetQuestionsByQuestionnaireIdAsync(int questionnaireId);
    Task<List<StandardQuestion>> GetStandardQuestionsAsync();
    Task<Question> AddQuestionAsync(Question question);
    Task<Question> GetQuestionAsync(int questionId);
    Task<Question> DeleteQuestionAsync(int questionId);
    Task<Question> UpdateQuestionAsync(Question question);
    Task<List<Question>> GetAllQuestionsAsync();
    Task<List<Question>> DeleteRangeAsync(List<Question> questions);
}
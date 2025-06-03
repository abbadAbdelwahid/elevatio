using EvaluationService.DTOs;
using EvaluationService.Models;

namespace EvaluationService.Services;

public interface IQuestionService
{
    Task<List<Question>> AddRangeAsync(List<CreateQuestionDto> questionsDto);
    Task<List<Question>> DeleteRangeAsync(List<Question> questions);
    Task<List<Question>> GetQuestionsByQuestionnaireIdAsync(int questionnaireId);
    Task<List<StandardQuestion>> GetStandardQuestionsAsync(int questionnaireId);
    Task<Question> AddQuestionAsync(CreateQuestionDto questionDto);
    Task<Question> GetQuestionAsync(int questionId);
    Task<Question> DeleteQuestionAsync(int questionId);
    Task<Question> UpdateQuestionAsync(CreateQuestionDto questionDto);
    Task<List<Question>> GetAllQuestionsAsync();
}
using EvaluationService.DTOs;
using EvaluationService.Models;

namespace EvaluationService.Repositories;

public interface IAnswerRepository
{
    Task<List<Answer>> AddRangeAsync(List<Answer> answers);
    Task<TypeModuleFiliere?> GetQuestionnaireTypeFiliereModuleAsync(int answerId);
    Task<TypeInternalExternal?> GetQuestionnaireTypeInternalExternalAsync(int answerId);
    Task<List<Answer>> GetAnswersFiliereAsync();
    Task<List<Answer>> GetAnswersModuleAsync();
    Task<List<Answer>> GetAnswersByQuestionIdAsync(int questionId);
    Task<List<Answer>> GetAnswersByQuestionnaireIdAsync(int questionnaireId);
    Task<List<Answer>> GetAnswersByRespondentIdAsync(string respondentId);
    Task<List<Answer>> GetAllAnswersAsync();
    Task<Answer> GetAnswerByIdAsync(int answerId);
    Task<Answer> DeleteAnswerByIdAsync(int answerId);
    Task<Answer> UpdateAnswerAsync(Answer answer);
    Task<Answer> AddAnswerAsync(Answer answer);
    Task<List<Answer>> DeleteRangeAsync(List<Answer> answers);
    Task<List<Answer>> DeleteAnswersByRespondentId(string respondentId);
    Task<List<Question?>> GetQuestionsOfAnswersList(List<Answer> answers);
    Task<Question> GetQuestionOfAnswerById(int answerId);
    Task<List<string?>> GetAllRespondentsIdsAsync();

}
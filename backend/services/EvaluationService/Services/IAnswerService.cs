using EvaluationService.DTOs;
using EvaluationService.Models;

namespace EvaluationService.Services;

public interface IAnswerService
{
    Task<List<Answer>> AddCleanRangeAsync(List<AnswerSubmissionDto> dtoAnswerSubmissions);
    Task<TypeInternalExternal?> GetQuestionnaireTypeInternalExternalAsync(int answerId);
    Task<TypeModuleFiliere?> GetQuestionnaireTypeFiliereModuleAsync(int answerId);
    Task<List<Answer>> GetAnswersFiliereAsync();
    Task<List<Answer>> GetAnswersModuleAsync();
    Task<List<Answer>> GetAnswersByQuestionIdAsync(int questionId);
    Task<List<Answer>> GetAnswersByQuestionnaireIdAsync(int questionnaireId);
    Task<List<Answer>> GetAnswersByRespondentIdAsync(string respondentId);
    Task<List<Answer>> GetAllAnswersAsync();
    Task<Answer> GetAnswerByIdAsync(int answerId);
    Task<Answer> DeleteAnswerByIdAsync(int answerId);
    Task<Answer> UpdateCleanAnswerAsync(Answer updatedAnswer);
    Task<Answer> AddCleanAnswerAsync(AnswerSubmissionDto answerSubmissionDto);
    Task<List<Question?>> GetQuestionsOfAnswersList(List<Answer> answers);
    Task<Question> GetQuestionOfAnswerById(int answerId);
    Task<List<Answer>> DeleteAnswersByRespondentId(string respondentId);

}


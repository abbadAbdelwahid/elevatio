using EvaluationService.DTOs;
using EvaluationService.Models;

namespace EvaluationService.Services;

public interface IAnswerService
{
    Task<List<AnswerResponseDto>> AddCleanRangeAsync(List<AnswerSubmissionDto> dtoAnswerSubmissions);
    Task<string> GetAnswerTypeFiliereModuleAsync(int answerId);
    Task<QuestionnaireType> GetQuestionnaireTypeInternalExternalAsync(int answerId);
    Task<List<AnswerResponseDto>> GetAnswersFiliereAsync();
    Task<List<AnswerResponseDto>> GetAnswersModuleAsync();
    Task<List<AnswerResponseDto>> GetAnswersByQuestionIdAsync(int questionId);
    Task<List<AnswerResponseDto>> GetAnswersByQuestionnaireIdAsync(int questionnaireId);
    Task<List<AnswerResponseDto>> GetAnswersByRespondentIdAsync(string respondentId);
    Task<List<AnswerResponseDto>> GetAllAnswersAsync();
    Task<AnswerResponseDto> GetAnswerByIdAsync(int answerId);
    Task<AnswerResponseDto> DeleteAnswerByIdAsync(int answerId);
    Task<AnswerResponseDto> UpdateCleanAnswerAsync(Answer updatedAnswer);
    Task<AnswerResponseDto> AddCleanAnswerAsync(AnswerSubmissionDto answerSubmissionDto);
    Task<List<Question>> GetQuestionsOfAnswersList(List<Answer> answers);
    Task<Question> GetQuestionOfAnswer(Answer answer);
    Task<Question> GetQuestionOfAnswerById(int answerId);

}


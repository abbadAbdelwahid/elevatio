using EvaluationService.DTOs;
using EvaluationService.Models;

namespace EvaluationService.Services;

public interface IAIAnalyzer
{
    Task<List<Answer>> AddCleanAnswersRangeAsync(List<AnswerSubmissionDto> answersSubmissionDto);
    Task<Answer> CleanAnswerUpdateAsync(AnswerSubmissionDto updatedAnswerSubmissionDto);
}
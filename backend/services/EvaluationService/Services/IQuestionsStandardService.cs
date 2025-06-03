using EvaluationService.DTOs;
using EvaluationService.Models;

namespace EvaluationService.Services;

public interface IQuestionsStandardService
{
    Task<List<StandardQuestion>> GetQuestionsStandardsByStatName(StatName statName);
    Task<StandardQuestion> GetStandardQuestionById(int id);
    Task<List<StandardQuestion>> GetStandardQuestions();
    Task<StandardQuestion> DeleteStandardQuestionById(int id);
    Task<List<StandardQuestion>> DeleteStandardQuestionsByStatName(StatName statName);
    Task<StandardQuestion> AddStandardQuestion(CreateStandardQuestionDto createStandardQuestionDto);
    Task<StandardQuestion> UpdateStandardQuestion(CreateStandardQuestionDto updateStandardQuestionDto);
}
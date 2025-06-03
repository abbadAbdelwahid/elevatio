using EvaluationService.Models;

namespace EvaluationService.Repositories;

public interface IQuestionsStandardRepository
{
    Task<List<StandardQuestion>> GetQuestionsStandardsByStatName(StatName statName);
    Task<StandardQuestion> GetStandardQuestionById(int id);
    Task<List<StandardQuestion>> GetStandardQuestions();
    Task<StandardQuestion> DeleteStandardQuestionById(int id);
    Task<List<StandardQuestion>> DeleteStandardQuestionsByStatName(StatName statName);
    Task<StandardQuestion> AddStandardQuestion(StandardQuestion standardQuestion);
    Task<StandardQuestion> UpdateStandardQuestion(StandardQuestion standardQuestion);
}
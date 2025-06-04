using EvaluationService.DTOs;
using EvaluationService.Models;

namespace EvaluationService.Services;

public interface IQuestionnaireService
{
    Task HandlingCascadeDeletion();
    Task<List<Question>> AddStandardQuestionsToQuestionnaireAsync(int questionnaireId,StatName statName);
    Task<List<Question>> AddQuestionsToQuestionnaireAsync(int questionnaireId, List<Question> questions);
    Task<List<StandardQuestion>> GetStandardQuestionsAsync(int questionnaireId);
    Task<QuestionnaireType> GetQuestionnaireTypeExternalInternalAsync(int questionnaireId);
    Task<List<Questionnaire>> GetQuestionnairesTypeExternalAsync();
    Task<List<Questionnaire>> GetQuestionnairesTypeInternalAsync();
    Task<string> GetQuestionnaireTypeModuleFiliereAsync(int questionnaireId);
    Task<Questionnaire?> GetQuestionnaireByIdAsync(int questionnaireId);
    Task<List<Questionnaire>> GetQuestionnairesByModuleIdAsync(int moduleId);
    Task<List<Questionnaire>> GetQuestionnairesByFiliereIdAsync(int filiereId);
    Task<List<Questionnaire>> GetQuestionnairesByCreatorUserIdAsync(string creatorUserId);
    Task<Questionnaire> DeleteQuestionnaireAsync(int questionnaireId);
    Task<List<Questionnaire>> DeleteQuestionnairesByRespondentIdAsync(string respondentId);
    Task<List<Questionnaire>> DeleteQuestionnairesByModuleIdAsync(int moduleId);
    Task<List<Questionnaire>> DeleteQuestionnairesByFiliereIdAsync(int filiereId);
    Task<List<Questionnaire>> DeleteQuestionnairesByCreatorIdAsync(string creatorId);
    Task<Questionnaire> UpdateQuestionnaireAsync(Questionnaire q);
    Task<Questionnaire> AddQuestionnaireAsync(CreateQuestionnaireDto q);
    Task<List<Questionnaire>> GetAllQuestionnairesAsync();
    Task<List<Questionnaire>> GetAllQuestionnairesFiliereAsync();
    Task<List<Questionnaire>> GetAllQuestionnairesModuleAsync();
    Task<List<Questionnaire>> GetAllQuestionnairesByCreatorUserIdAsync(string creatorUserId);
    Task<string?> GetRespondentIdByQuestionnaireIdAsync(int questionnaireId);
    Task<List<Questionnaire>> GetQuestionnairesByRespondentIdAsync(string respondentId);
}
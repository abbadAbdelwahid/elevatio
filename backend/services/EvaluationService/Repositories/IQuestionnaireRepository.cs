using EvaluationService.Models;

namespace EvaluationService.Repositories;

public interface IQuestionnaireRepository
{
    Task<List<Question>> AddStandardQuestionsToQuestionnaireAsync(int questionnaireId,StatName statName);
    Task<List<Question>> AddQuestionsToQuestionnaireAsync(int questionnaireId, List<Question> questions);
    Task<List<StandardQuestion>> GetStandardQuestionsAsync(int questionnaireId);
    Task<TypeInternalExternal?> GetQuestionnaireTypeExternalInternalAsync(int questionnaireId);
    Task<List<Questionnaire>> GetQuestionnairesTypeExternalAsync();
    Task<List<Questionnaire>> GetQuestionnairesTypeInternalAsync();
    Task<TypeModuleFiliere?> GetQuestionnaireTypeModuleFiliereAsync(int questionnaireId);
    Task<List<Questionnaire>> GetQuestionnairesTypeModuleAsync();
    Task<List<Questionnaire>> GetQuestionnairesTypeFiliereAsync();
    Task<Questionnaire> GetQuestionnaireByIdAsync(int questionnaireId);
    Task<List<Questionnaire>> GetQuestionnairesByModuleIdAsync(int moduleId);
    Task<List<Questionnaire>> GetQuestionnairesByFiliereIdAsync(int filiereId);
    Task<List<Questionnaire>> GetQuestionnairesByCreatorUserIdAsync(string creatorUserId);
    Task<Questionnaire> DeleteQuestionnaireAsync(int questionnaireId);
    Task<List<Questionnaire>> DeleteQuestionnairesByModuleIdAsync(int moduleId);
    Task<List<Questionnaire>> DeleteQuestionnairesByFiliereIdAsync(int filiereId);
    Task<List<Questionnaire>> DeleteQuestionnairesByCreatorIdAsync(string creatorId);
    Task<Questionnaire> UpdateQuestionnaireAsync(Questionnaire q);
    Task<Questionnaire> AddQuestionnaireAsync(Questionnaire q);
    Task<List<Questionnaire>> GetAllQuestionnairesAsync();
    Task<List<Questionnaire>> GetAllQuestionnairesFiliereAsync();
    Task<List<Questionnaire>> GetAllQuestionnairesModuleAsync();
    Task<List<Questionnaire>> GetAllQuestionnairesByCreatorUserIdAsync(string creatorUserId);
    Task<List<string?>> GetRespondentsIdsByQuestionnaireIdAsync(int questionnaireId);
    Task<List<Questionnaire>> GetQuestionnairesByRespondentIdAsync(string respondentId);

    Task<List<Questionnaire>> DeleteRangeAsync(List<Questionnaire> questionnaires);
}
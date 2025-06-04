using AutoMapper;
using EvaluationService.Data;
using EvaluationService.DTOs;
using EvaluationService.Models;
using EvaluationService.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace EvaluationService.Services;

public class QuestionnaireService : IQuestionnaireService
{
    private readonly IQuestionnaireRepository _questionnaireRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly IExternalValidationService _externalValidationService;
    private readonly IMapper _mapper;

    public QuestionnaireService(
        IQuestionnaireRepository questionnaireRepository,
        IQuestionRepository questionRepository,
        IExternalValidationService externalValidationService,
        IMapper mapper)
    {
        _questionnaireRepository = questionnaireRepository;
        _questionRepository = questionRepository;
        _externalValidationService = externalValidationService;
        _mapper = mapper;
    }

    public async Task HandlingCascadeDeletion()
    {
        try
        {
            var questionnaires = await _questionnaireRepository.GetAllQuestionnairesAsync();

            var creatorUsers = questionnaires
                .Where(q => !string.IsNullOrWhiteSpace(q.CreatorUserId))
                .Select(q => q.CreatorUserId)
                .ToList();

            var respondentUsers = questionnaires
                .Where(q => !string.IsNullOrWhiteSpace(q.RespondentUserId))
                .Select(q => q.RespondentUserId)
                .ToList();

            var filieresIds = questionnaires
                .Where(q => q.FiliereId != null)
                .Select(q => q.FiliereId)
                .ToList();

            var modulesIds = questionnaires
                .Where(q => q.ModuleId != null)
                .Select(q => q.ModuleId)
                .ToList();

            var allUsers = creatorUsers.Concat(respondentUsers).Distinct().ToList();

            // External validation
            var modulesExist  = await _externalValidationService.ModulesExistAsync(modulesIds);
            var filieresExist = await _externalValidationService.FilieresExistAsync(filieresIds);
            var usersExist  = await _externalValidationService.UsersExistAsync(allUsers);

            var questionnairesToDelete = new List<Questionnaire>();

            foreach (var questionnaire in questionnaires)
            {
                var moduleOk = questionnaire.ModuleId == null ||
                               modulesExist.GetValueOrDefault(questionnaire.ModuleId.Value, true);

                var filiereOk = questionnaire.FiliereId == null ||
                                filieresExist.GetValueOrDefault(questionnaire.FiliereId.Value, true);

                var creatorOk = string.IsNullOrEmpty(questionnaire.CreatorUserId) ||
                                usersExist.GetValueOrDefault(questionnaire.CreatorUserId, true);

                var respondentOk = string.IsNullOrEmpty(questionnaire.RespondentUserId) ||
                                   usersExist.GetValueOrDefault(questionnaire.RespondentUserId, true);

                if (!moduleOk || !filiereOk || !creatorOk || !respondentOk)
                {
                    questionnairesToDelete.Add(questionnaire);
                }
            }

            await _questionnaireRepository.DeleteRangeAsync(questionnairesToDelete);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error in HandlingCascadeDeletion: {e.Message}");
        }
    }

    public async Task<List<Question>> AddStandardQuestionsToQuestionnaireAsync(int questionnaireId, StatName statName)
    {
        // await HandlingCascadeDeletion();
        return await _questionnaireRepository.AddStandardQuestionsToQuestionnaireAsync(questionnaireId, statName);
    }

    public async Task<List<Question>> AddQuestionsToQuestionnaireAsync(int questionnaireId, List<Question> questions)
    {
        // await HandlingCascadeDeletion();
        return await _questionnaireRepository.AddQuestionsToQuestionnaireAsync(questionnaireId, questions);
    }

    public async Task<List<StandardQuestion>> GetStandardQuestionsAsync(int questionnaireId)
    {
        // await HandlingCascadeDeletion();
        return await _questionnaireRepository.GetStandardQuestionsAsync(questionnaireId);
    }

    public async Task<QuestionnaireType> GetQuestionnaireTypeExternalInternalAsync(int questionnaireId)
    {
        // await HandlingCascadeDeletion();
        return await _questionnaireRepository.GetQuestionnaireTypeExternalInternalAsync(questionnaireId);
    }

    public async Task<List<Questionnaire>> GetQuestionnairesTypeExternalAsync()
    {
        // await HandlingCascadeDeletion();
        return await _questionnaireRepository.GetQuestionnairesTypeExternalAsync();
    }

    public async Task<List<Questionnaire>> GetQuestionnairesTypeInternalAsync()
    {
        // await HandlingCascadeDeletion();
        return await _questionnaireRepository.GetQuestionnairesTypeInternalAsync();   
    }

    public async Task<string> GetQuestionnaireTypeModuleFiliereAsync(int questionnaireId)
    {
        // await HandlingCascadeDeletion();
        return await _questionnaireRepository.GetQuestionnaireTypeModuleFiliereAsync(questionnaireId);
    }

    public async Task<Questionnaire?> GetQuestionnaireByIdAsync(int questionnaireId)
    {
        // await HandlingCascadeDeletion();
        return await _questionnaireRepository.GetQuestionnaireByIdAsync(questionnaireId);   
    }

    public async Task<List<Questionnaire>> GetQuestionnairesByModuleIdAsync(int moduleId)
    {
        // await HandlingCascadeDeletion();
        return await _questionnaireRepository.GetQuestionnairesByModuleIdAsync(moduleId);   
    }

    public async Task<List<Questionnaire>> GetQuestionnairesByFiliereIdAsync(int filiereId)
    {
        // await HandlingCascadeDeletion();
        return await _questionnaireRepository.GetQuestionnairesByFiliereIdAsync(filiereId);  
    }

    public async Task<List<Questionnaire>> GetQuestionnairesByCreatorUserIdAsync(string creatorUserId)
    {
        // await HandlingCascadeDeletion();
        return await _questionnaireRepository.GetQuestionnairesByCreatorUserIdAsync(creatorUserId);  
    }

    public async Task<Questionnaire> DeleteQuestionnaireAsync(int questionnaireId)
    {
        // await HandlingCascadeDeletion();
        return await _questionnaireRepository.DeleteQuestionnaireAsync(questionnaireId); 
    }

    public async Task<List<Questionnaire>> DeleteQuestionnairesByRespondentIdAsync(string respondentId)
    {
        // await HandlingCascadeDeletion();
        return await _questionnaireRepository.DeleteQuestionnairesByRespondentIdAsync(respondentId);
    }

    public async Task<List<Questionnaire>> DeleteQuestionnairesByModuleIdAsync(int moduleId)
    {
        // await HandlingCascadeDeletion();
        return await _questionnaireRepository.DeleteQuestionnairesByModuleIdAsync(moduleId);
    }

    public async Task<List<Questionnaire>> DeleteQuestionnairesByFiliereIdAsync(int filiereId)
    {
        // await HandlingCascadeDeletion();
        return await _questionnaireRepository.DeleteQuestionnairesByFiliereIdAsync(filiereId);
    }

    public async Task<List<Questionnaire>> DeleteQuestionnairesByCreatorIdAsync(string creatorId)
    {
        // await HandlingCascadeDeletion();
        return await _questionnaireRepository.DeleteQuestionnairesByCreatorIdAsync(creatorId);
    }

    public async Task<Questionnaire> UpdateQuestionnaireAsync(Questionnaire q)
    {
        // await HandlingCascadeDeletion();
        return await _questionnaireRepository.UpdateQuestionnaireAsync(q);
    }

    public async Task<Questionnaire> AddQuestionnaireAsync(CreateQuestionnaireDto q)
    {
        // await HandlingCascadeDeletion();
        return await _questionnaireRepository.AddQuestionnaireAsync(_mapper.Map<Questionnaire>(q));
    }

    public async Task<List<Questionnaire>> GetAllQuestionnairesAsync()
    {
        // await HandlingCascadeDeletion();
        return await _questionnaireRepository.GetAllQuestionnairesAsync(); 
    }

    public async Task<List<Questionnaire>> GetAllQuestionnairesFiliereAsync()
    {
        // await HandlingCascadeDeletion();
        return await _questionnaireRepository.GetAllQuestionnairesFiliereAsync();
    }

    public async Task<List<Questionnaire>> GetAllQuestionnairesModuleAsync()
    {
        // await HandlingCascadeDeletion();
        return await _questionnaireRepository.GetAllQuestionnairesModuleAsync();
    }

    public async Task<List<Questionnaire>> GetAllQuestionnairesByCreatorUserIdAsync(string creatorUserId)
    {
        // await HandlingCascadeDeletion();
        return await _questionnaireRepository.GetAllQuestionnairesByCreatorUserIdAsync(creatorUserId);
    }

    public async Task<string?> GetRespondentIdByQuestionnaireIdAsync(int questionnaireId)
    {
        // await HandlingCascadeDeletion();
        return await _questionnaireRepository.GetRespondentIdByQuestionnaireIdAsync(questionnaireId);
    }

    public async Task<List<Questionnaire>> GetQuestionnairesByRespondentIdAsync(string respondentId)
    {
        // await HandlingCascadeDeletion();
        return await _questionnaireRepository.GetQuestionnairesByRespondentIdAsync(respondentId);
    }
}

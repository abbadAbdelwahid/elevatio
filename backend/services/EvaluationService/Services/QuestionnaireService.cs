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
    private readonly IQuestionsStandardRepository _questionsStandardRepository;
    private readonly IAnswerRepository _answerRepository;
    private readonly IExternalValidationService _externalValidationService;
    private readonly IMapper _mapper;

    public QuestionnaireService(
        IQuestionnaireRepository questionnaireRepository,
        IQuestionRepository questionRepository,
        IQuestionsStandardRepository questionsStandardRepository,
        IAnswerRepository answerRepository,
        IExternalValidationService externalValidationService,
        IMapper mapper)
    {
        _questionnaireRepository = questionnaireRepository;
        _questionRepository = questionRepository;
        _questionsStandardRepository = questionsStandardRepository;
        _answerRepository = answerRepository;
        _externalValidationService = externalValidationService;
        _mapper = mapper;
    }

    public async Task HandlingCascadeDeletion()
    {
        try
        {
            var questionnaires = await _questionnaireRepository.GetAllQuestionnairesAsync();
            var answers = await _answerRepository.GetAllAnswersAsync();
            
            var creatorUsers = questionnaires
                .Where(q => !string.IsNullOrWhiteSpace(q.CreatorUserId))
                .Select(q => q.CreatorUserId)
                .ToList();

            var respondentUsers = await _answerRepository.GetAllRespondentsIdsAsync();

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

            var answersToDelete = new List<Answer>();
            foreach (var answer in answers)
            {
                if (answer.RespondentUserId == null ||
                    usersExist.GetValueOrDefault(answer.RespondentUserId, true))
                {
                    answersToDelete.Add(answer);
                }
            }
            await _answerRepository.DeleteRangeAsync(answersToDelete);
            
            var questionnairesToDelete = new List<Questionnaire>();

            foreach (var questionnaire in questionnaires)
            {
                var moduleOk = questionnaire.ModuleId == null ||
                               modulesExist.GetValueOrDefault(questionnaire.ModuleId.Value, true);

                var filiereOk = questionnaire.FiliereId == null ||
                                filieresExist.GetValueOrDefault(questionnaire.FiliereId.Value, true);

                var creatorOk = string.IsNullOrEmpty(questionnaire.CreatorUserId) ||
                                usersExist.GetValueOrDefault(questionnaire.CreatorUserId, true);

                if (!moduleOk || !filiereOk || !creatorOk)
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

    public async Task<List<Question>> AddQuestionsToQuestionnaireAsync(int questionnaireId, List<CreateQuestionWithQuestionnaire> questionsDto)
    {
        // await HandlingCascadeDeletion();
        return await _questionnaireRepository.AddQuestionsToQuestionnaireAsync(questionnaireId, _mapper.Map<List<Question>>(questionsDto));;
    }

    public async Task<List<StandardQuestion>> GetStandardQuestionsAsync(int questionnaireId)
    {
        // await HandlingCascadeDeletion();
        return await _questionnaireRepository.GetStandardQuestionsAsync(questionnaireId);
    }

    public async Task<TypeInternalExternal?> GetQuestionnaireTypeExternalInternalAsync(int questionnaireId)
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

    public async Task<TypeModuleFiliere?> GetQuestionnaireTypeModuleFiliereAsync(int questionnaireId)
    {
        // await HandlingCascadeDeletion();
        return await _questionnaireRepository.GetQuestionnaireTypeModuleFiliereAsync(questionnaireId);
    }
    
    public async Task<List<Questionnaire>> GetQuestionnairesTypeModuleAsync()
    {
        return await _questionnaireRepository.GetQuestionnairesTypeModuleAsync();
    }
    
    public async Task<List<Questionnaire>> GetQuestionnairesTypeFiliereAsync()
    {
        return await _questionnaireRepository.GetQuestionnairesTypeFiliereAsync();
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

    public async Task<Questionnaire> AddQuestionnaireAsync(CreateQuestionnaireDto dto)
    {
        // await HandlingCascadeDeletion();
        
        var q = _mapper.Map<Questionnaire>(dto);
        q.Questions = new List<Question>();
        if (dto.Questions != null)
        {
            foreach (var cq in dto.Questions)
            {
                if(cq.StandardQuestionId.HasValue && cq.Text == null)
                {
                    var sq = await _questionsStandardRepository.GetStandardQuestionById(cq.StandardQuestionId.Value);
                    cq.Text = sq.Text;
                }

                q.Questions.Add(new Question
                {
                    StandardQuestionId = cq.StandardQuestionId,
                    Text               = cq.Text,
                    CreatedAt          = DateTime.UtcNow,
                });
            }
        }
        return await _questionnaireRepository.AddQuestionnaireAsync(q);
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

    public async Task<List<string?>> GetRespondentsIdsByQuestionnaireIdAsync(int questionnaireId)
    {
        // await HandlingCascadeDeletion();
        return await _questionnaireRepository.GetRespondentsIdsByQuestionnaireIdAsync(questionnaireId);
    }

    public async Task<List<Questionnaire>> GetQuestionnairesByRespondentIdAsync(string respondentId)
    {
        // await HandlingCascadeDeletion();
        return await _questionnaireRepository.GetQuestionnairesByRespondentIdAsync(respondentId);
    }
    
    public async Task<List<Questionnaire>> DeleteRangeAsync(List<Questionnaire> questionnaires)
    {
        return await _questionnaireRepository.DeleteRangeAsync(questionnaires);
    }
    

}

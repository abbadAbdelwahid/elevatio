using AutoMapper;
using EvaluationService.Data;
using EvaluationService.Models;
using EvaluationService.Services;
using Microsoft.EntityFrameworkCore;

namespace EvaluationService.Repositories;

public class QuestionnaireRepository : IQuestionnaireRepository
{
    private readonly EvaluationsDbContext _ctx;
    private readonly IQuestionRepository _questionRepository;
    private readonly IQuestionsStandardRepository _questionsStandardRepository;
    private readonly IMapper _mapper;

    public QuestionnaireRepository(EvaluationsDbContext ctx, IQuestionRepository questionRepository,
                IQuestionsStandardRepository questionsStandardRepository, IMapper mapper)
    {
        _ctx = ctx;
        _questionRepository = questionRepository;
        _questionsStandardRepository = questionsStandardRepository;
        _mapper = mapper;
    }


    public async Task<List<Question>> AddStandardQuestionsToQuestionnaireAsync(int questionnaireId, StatName statName)
    {
        List<StandardQuestion> standardQuestions = await _questionsStandardRepository.GetQuestionsStandardsByStatName(statName);
        var nouvellesQuestionsStandards = new List<Question>();
        foreach (StandardQuestion sq in standardQuestions)
        {
            nouvellesQuestionsStandards.Add(
                new Question
            {
                QuestionnaireId = questionnaireId,
                CreatedAt = DateTime.UtcNow,
                StandardQuestionId = sq.StandardQuestionId,
                Text = sq.Text,
            });
        }
        List<Question> questionInseres = await _questionRepository.AddRangeAsync(nouvellesQuestionsStandards);
        return questionInseres;
    }

    public async Task<List<Question>> AddQuestionsToQuestionnaireAsync(int questionnaireId, List<Question> questions)
    {
        var q = questions.Select(q =>
        {
            q.QuestionnaireId = questionnaireId;
            return q;
        }).ToList();
        return await _questionRepository.AddRangeAsync(questions);
    }

    public async Task<List<StandardQuestion>> GetStandardQuestionsAsync(int questionnaireId)
    {
        var questionOfQuestionnaire = await _questionRepository.GetQuestionsByQuestionnaireIdAsync(questionnaireId);
        List<StandardQuestion> standardQuestions = new List<StandardQuestion>();
        foreach (var qStand in questionOfQuestionnaire)
        {
            standardQuestions.Add(await _questionsStandardRepository.GetStandardQuestionById((int)qStand!.StandardQuestionId));
        }
        return standardQuestions;
    }

    public async Task<TypeInternalExternal?> GetQuestionnaireTypeExternalInternalAsync(int questionnaireId)
    {
        return await _ctx.Questionnaires.AsNoTracking()
            .Where(q => q.QuestionnaireId == questionnaireId)
            .Select(q => q.TypeInternalExternal)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Questionnaire>> GetQuestionnairesTypeExternalAsync()
    {
        return await _ctx.Questionnaires.AsNoTracking()
            .Where(q => q.TypeInternalExternal == TypeInternalExternal.External)
            .Include(q => q!.Questions)
                .ThenInclude(q => q!.StandardQuestion)
            .Include(q => q!.Questions)
                .ThenInclude(q => q.Answers)
            .AsSplitQuery()
            .ToListAsync();
    }

    public async Task<List<Questionnaire>> GetQuestionnairesTypeInternalAsync()
    {
        return await _ctx.Questionnaires.AsNoTracking()
            .Where(q => q.TypeInternalExternal == TypeInternalExternal.Internal)
            .Include(q => q!.Questions)
                .ThenInclude(q => q!.StandardQuestion)
            .Include(q => q!.Questions)
                .ThenInclude(q => q.Answers)
            .AsSplitQuery()
            .ToListAsync();    
    }

    public async Task<TypeModuleFiliere?> GetQuestionnaireTypeModuleFiliereAsync(int questionnaireId)
    {
        var questionnaireCherche = await GetQuestionnaireByIdAsync(questionnaireId);
        return questionnaireCherche.TypeModuleFiliere;
    }

    public async Task<List<Questionnaire>> GetQuestionnairesTypeModuleAsync()
    {
        return await _ctx.Questionnaires.AsNoTracking()
            .Where(q => q.TypeModuleFiliere == TypeModuleFiliere.Module)
            .Include(q => q!.Questions)
                .ThenInclude(q => q!.StandardQuestion)
            .Include(q => q!.Questions)
                .ThenInclude(q => q.Answers)
            .AsSplitQuery()
            .ToListAsync();   
    }

    public async Task<List<Questionnaire>> GetQuestionnairesTypeFiliereAsync()
    {
        return await _ctx.Questionnaires.AsNoTracking()
            .Where(q => q.TypeModuleFiliere == TypeModuleFiliere.Filiere)
            .Include(q => q!.Questions)
                .ThenInclude(q => q!.StandardQuestion)
            .Include(q => q!.Questions)
                .ThenInclude(q => q.Answers)
            .AsSplitQuery()
            .ToListAsync();   
    }

    public async Task<Questionnaire> GetQuestionnaireByIdAsync(int questionnaireId)
    {
        return await _ctx.Questionnaires.AsNoTracking()
            .Where(q => q.QuestionnaireId == questionnaireId)
            .Include(q => q!.Questions)
                .ThenInclude(q => q!.StandardQuestion)
            .Include(q => q!.Questions)
                .ThenInclude(q => q.Answers)
                .AsSplitQuery()
            .FirstOrDefaultAsync()
            ?? throw new KeyNotFoundException("Questionnaire not found");
    }

    public async Task<List<Questionnaire>> GetQuestionnairesByModuleIdAsync(int moduleId)
    {
        return await _ctx.Questionnaires.AsNoTracking()
            .Where(q => q.ModuleId == moduleId)
            .Include(q => q!.Questions)
            .ThenInclude(q => q!.StandardQuestion)
            .Include(q => q!.Questions)
            .ThenInclude(q => q.Answers)
            .AsSplitQuery()
            .ToListAsync();
    }

    public async Task<List<Questionnaire>> GetQuestionnairesByFiliereIdAsync(int filiereId)
    {
        return await _ctx.Questionnaires.AsNoTracking()
            .Where(q => q.FiliereId == filiereId)
            .Include(q => q!.Questions)
            .ThenInclude(q => q!.StandardQuestion)
            .Include(q => q!.Questions)
            .ThenInclude(q => q.Answers)
            .AsSplitQuery()
            .ToListAsync();
    }

    public async Task<List<Questionnaire>> GetQuestionnairesByCreatorUserIdAsync(string creatorUserId)
    {
        return await _ctx.Questionnaires.AsNoTracking()
            .Where(q => q.CreatorUserId != null &&
                        q.CreatorUserId.ToLower() == creatorUserId.ToLower())
            .Include(q => q!.Questions)
            .ThenInclude(q => q!.StandardQuestion)
            .Include(q => q!.Questions)
            .ThenInclude(q => q.Answers)
            .AsSplitQuery()
            .ToListAsync();
    }

    public async Task<Questionnaire> DeleteQuestionnaireAsync(int questionnaireId)
    {
        var questionnaireASupprimer = await GetQuestionnaireByIdAsync(questionnaireId);
        if (questionnaireASupprimer != null)
        {
            _ctx.Questionnaires.Remove(questionnaireASupprimer);
            await _ctx.SaveChangesAsync();
            return questionnaireASupprimer;
        }
        throw new KeyNotFoundException("Questionnaire not found");
    }

    public async Task<List<Questionnaire>> DeleteQuestionnairesByModuleIdAsync(int moduleId)
    {
        var questionnairesOfModule = await GetQuestionnairesByModuleIdAsync(moduleId);
        _ctx.Questionnaires.RemoveRange(questionnairesOfModule);
        await _ctx.SaveChangesAsync();
        return questionnairesOfModule;
    }

    public async Task<List<Questionnaire>> DeleteQuestionnairesByFiliereIdAsync(int filiereId)
    {
        var questionnairesOfFiliere = await GetQuestionnairesByFiliereIdAsync(filiereId);
        _ctx.Questionnaires.RemoveRange(questionnairesOfFiliere);
        await _ctx.SaveChangesAsync();
        return questionnairesOfFiliere;
    }

    public async Task<List<Questionnaire>> DeleteQuestionnairesByCreatorIdAsync(string creatorId)
    {
        var questionnairesOfCreator = await GetQuestionnairesByCreatorUserIdAsync(creatorId);
        _ctx.Questionnaires.RemoveRange(questionnairesOfCreator);
        await _ctx.SaveChangesAsync();
        return questionnairesOfCreator;
    }

    public async Task<Questionnaire> UpdateQuestionnaireAsync(Questionnaire q)
    {
        _ctx.Questionnaires.Update(q);
        await _ctx.SaveChangesAsync();
        return await GetQuestionnaireByIdAsync(q.QuestionnaireId);
    }

    public async Task<Questionnaire> AddQuestionnaireAsync(Questionnaire q)
    {
        await _ctx.Questionnaires.AddAsync(q);
        await _ctx.SaveChangesAsync();
        return await GetQuestionnaireByIdAsync(q.QuestionnaireId);
    }

    public async Task<List<Questionnaire>> GetAllQuestionnairesAsync()
    {
        return await _ctx.Questionnaires.AsNoTracking()
            .Include(q => q!.Questions)
                .ThenInclude(q => q!.StandardQuestion)
            .Include(q => q!.Questions)
                .ThenInclude(q => q.Answers)
            .AsSplitQuery()
            .ToListAsync();
    }

    public async Task<List<Questionnaire>> GetAllQuestionnairesFiliereAsync()
    {
        return await _ctx.Questionnaires.AsNoTracking()
            .Where(q => q.TypeModuleFiliere == TypeModuleFiliere.Filiere)
            .Include(q => q!.Questions)
                .ThenInclude(q => q!.StandardQuestion)
            .Include(q => q!.Questions)
                .ThenInclude(q => q.Answers)
            .AsSplitQuery()
            .ToListAsync();
    }

    public async Task<List<Questionnaire>> GetAllQuestionnairesModuleAsync()
    {
        return await _ctx.Questionnaires.AsNoTracking()
            .Where(q => q.TypeModuleFiliere == TypeModuleFiliere.Module)
            .Include(q => q!.Questions)
                .ThenInclude(q => q!.StandardQuestion)
            .Include(q => q!.Questions)
                .ThenInclude(q => q.Answers)
            .AsSplitQuery()
            .ToListAsync();
    }

    public async Task<List<Questionnaire>> GetAllQuestionnairesByCreatorUserIdAsync(string creatorUserId)
    {
        return await _ctx.Questionnaires.AsNoTracking()
            .Where(q => q.CreatorUserId != null &&
                        q.CreatorUserId.ToLower() == creatorUserId.ToLower())
            .Include(q => q!.Questions)
                .ThenInclude(q => q!.StandardQuestion)
            .Include(q => q!.Questions)
                .ThenInclude(q => q.Answers)
            .AsSplitQuery()
            .ToListAsync();
    }

    public async Task<List<string?>> GetRespondentsIdsByQuestionnaireIdAsync(int questionnaireId)
    {
        var respondentsIds = await _ctx.Answers
            .Where(a=> a!.Question.QuestionnaireId == questionnaireId)
            .Select(answer => answer.RespondentUserId)
            .Distinct()
            .ToListAsync();
        
        return respondentsIds;
    }

    public async Task<List<Questionnaire>> GetQuestionnairesByRespondentIdAsync(string respondentId)
    {
        var listQuestionnaires = await _ctx.Answers.AsNoTracking()
            .Where(a => a!.RespondentUserId == respondentId)
            .Select(a => a!.Question.Questionnaire)
            .Distinct()
            .ToListAsync();

        var questionnairesTask = listQuestionnaires
            .Select(q => GetQuestionnaireByIdAsync(q.QuestionnaireId))
            .ToList();
        
        var questionnaires = await Task.WhenAll(questionnairesTask);
        
        return questionnaires.ToList();
    }


    public async Task<List<Questionnaire>> DeleteRangeAsync(List<Questionnaire> questionnaires)
    {
        _ctx.Questionnaires.RemoveRange(questionnaires);
        await _ctx.SaveChangesAsync();
        return questionnaires;
    }

}
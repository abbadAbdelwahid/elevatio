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
        Questionnaire? questionnaire = GetQuestionnaireByIdAsync(questionnaireId).Result;
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
        await AddQuestionsToQuestionnaireAsync(questionnaireId, questionInseres);
        return questionInseres;
    }

    public async Task<List<Question>> AddQuestionsToQuestionnaireAsync(int questionnaireId, List<Question> questions)
    {
        List<Question> questionInseres = new List<Question>();
        foreach (Question q in questions)
        {
            q.QuestionnaireId = questionnaireId;
            await _questionRepository.AddQuestionAsync(q);
            questionInseres.Add(q);
        }
        return questionInseres;
    }

    public async Task<List<StandardQuestion>> GetStandardQuestionsAsync(int questionnaireId)
    {
        var questionOfQuestionnaire = await _questionRepository.GetQuestionsByQuestionnaireIdAsync(questionnaireId);
        List<StandardQuestion> standardQuestions = new List<StandardQuestion>();
        foreach (var qStand in questionOfQuestionnaire)
        {
            standardQuestions.Add(_questionsStandardRepository.GetStandardQuestionById((int)qStand.StandardQuestionId).Result);
        }
        return standardQuestions;
    }

    public async Task<QuestionnaireType> GetQuestionnaireTypeExternalInternalAsync(int questionnaireId)
    {
        return await _ctx.Questionnaires.AsNoTracking()
            .Where(q => q.QuestionnaireId == questionnaireId)
            .Select(q => q.Type)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Questionnaire>> GetQuestionnairesTypeExternalAsync()
    {
        return await _ctx.Questionnaires.AsNoTracking()
            .Where(q => q.Type == QuestionnaireType.External)
            .ToListAsync();
    }

    public async Task<List<Questionnaire>> GetQuestionnairesTypeInternalAsync()
    {
        return await _ctx.Questionnaires.AsNoTracking()
            .Where(q => q.Type == QuestionnaireType.Internal)
            .ToListAsync();    
    }

    public async Task<string> GetQuestionnaireTypeModuleFiliereAsync(int questionnaireId)
    {
        var questionnaireCherche = await GetQuestionnaireByIdAsync(questionnaireId);
        if (questionnaireCherche.FiliereId != null && questionnaireCherche.ModuleId == null)
            return "Filiere";
        if(questionnaireCherche.FiliereId == null && questionnaireCherche.ModuleId != null)
            return "Module";
        return "Invalid_Questionnaire_Type";
    }

    public async Task<Questionnaire> GetQuestionnaireByIdAsync(int questionnaireId)
    {
        return await _ctx.Questionnaires.AsNoTracking()
            .Where(q => q.QuestionnaireId == questionnaireId)
            .FirstOrDefaultAsync()
            ?? throw new KeyNotFoundException("Questionnaire not found");
    }

    public async Task<List<Questionnaire>> GetQuestionnairesByModuleIdAsync(int moduleId)
    {
        return await _ctx.Questionnaires.AsNoTracking()
            .Where(q => q.ModuleId == moduleId)
            .ToListAsync();
    }

    public async Task<List<Questionnaire>> GetQuestionnairesByFiliereIdAsync(int filiereId)
    {
        return await _ctx.Questionnaires.AsNoTracking()
            .Where(q => q.FiliereId == filiereId)
            .ToListAsync();
    }

    public async Task<List<Questionnaire>> GetQuestionnairesByCreatorUserIdAsync(string creatorUserId)
    {
        return await _ctx.Questionnaires.AsNoTracking()
            .Where(q => q.CreatorUserId != null &&
                        q.CreatorUserId.ToLower() == creatorUserId.ToLower())
            .ToListAsync();
    }

    public async Task<Questionnaire> DeleteQuestionnaireAsync(int questionnaireId)
    {
        var questionnaireASupprimer = GetQuestionnaireByIdAsync(questionnaireId).Result;
        if (questionnaireASupprimer != null)
        {
            _ctx.Questionnaires.Remove(questionnaireASupprimer);
            await _ctx.SaveChangesAsync();
            return questionnaireASupprimer;
        }
        throw new KeyNotFoundException("Questionnaire not found");
    }

    public async Task<Questionnaire> UpdateQuestionnaireAsync(Questionnaire q)
    {
        _ctx.Questionnaires.Update(q);
        await _ctx.SaveChangesAsync();
        return q;
    }

    public async Task<Questionnaire> AddQuestionnaireAsync(Questionnaire q)
    {
        await _ctx.Questionnaires.AddAsync(q);
        await _ctx.SaveChangesAsync();
        return q;
    }

    public async Task<List<Questionnaire>> GetAllQuestionnairesAsync()
    {
        return await _ctx.Questionnaires.AsNoTracking().ToListAsync();
    }

    public async Task<List<Questionnaire>> GetAllQuestionnairesFiliereAsync()
    {
        return await _ctx.Questionnaires.AsNoTracking()
            .Where(q => q.FiliereId != null && q.ModuleId == null)
            .ToListAsync();
    }

    public async Task<List<Questionnaire>> GetAllQuestionnairesModuleAsync()
    {
        return await _ctx.Questionnaires.AsNoTracking()
            .Where(q => q.FiliereId == null && q.ModuleId != null)
            .ToListAsync();
    }

    public async Task<List<Questionnaire>> GetAllQuestionnairesByCreatorUserIdAsync(string creatorUserId)
    {
        return await _ctx.Questionnaires.AsNoTracking()
            .Where(q => q.CreatorUserId != null &&
                        q.CreatorUserId.ToLower() == creatorUserId.ToLower())
            .ToListAsync();
    }

    public async Task<string?> GetRespondentIdByQuestionnaireIdAsync(int questionnaireId)
    {
        return await _ctx.Questionnaires.AsNoTracking()
            .Where(q => q.QuestionnaireId == questionnaireId)
            .Select(q => q.RespondentUserId)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Questionnaire>> GetQuestionnairesByRespondentIdAsync(string respondentId)
    {
        return await _ctx.Questionnaires.AsNoTracking()
            .Where(q => q.RespondentUserId != null &&
                        q.RespondentUserId.ToLower() == respondentId.ToLower())
            .ToListAsync();
    }

    public async Task<List<Questionnaire>> DeleteRangeAsync(List<Questionnaire> questionnaires)
    {
        _ctx.Questionnaires.RemoveRange(questionnaires);
        await _ctx.SaveChangesAsync();
        return questionnaires;
    }

}
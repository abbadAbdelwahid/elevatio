using AutoMapper;
using EvaluationService.DTOs;
using EvaluationService.Models;
using EvaluationService.Repositories;

namespace EvaluationService.Services;

public class AnswerService : IAnswerService
{
    private readonly IAnswerRepository _answerRepository;
    private readonly IEvaluationRepository _evaluationRepository;
    private readonly IExternalValidationService _externalValidationService;
    private readonly IAIAnalyzer _aiAnalyzer;
    private readonly HttpClient _client;
    private readonly IMapper _mapper;

    public AnswerService(
        IAnswerRepository answerRepository,
        IEvaluationRepository evaluationRepository,
        IConfiguration cfg,
        IExternalValidationService externalValidationService,
        HttpClient client,
        IAIAnalyzer aiAnalyzer,
        IMapper mapper
        )
    {
        _answerRepository = answerRepository;
        _evaluationRepository = evaluationRepository;
        _externalValidationService = externalValidationService;
        _client = client;
        _aiAnalyzer = aiAnalyzer;
        _mapper = mapper;
    }

    public async Task<List<Answer>> AddCleanRangeAsync(List<AnswerSubmissionDto> dtoAnswersSubmission)
    {
        List<Answer> answers = await _aiAnalyzer.AddCleanAnswersRangeAsync(dtoAnswersSubmission);
        return _mapper.Map<List<Answer>>(answers);
    }

    public async Task<TypeModuleFiliere?> GetQuestionnaireTypeFiliereModuleAsync(int answerId)
    {
        return await _answerRepository.GetQuestionnaireTypeFiliereModuleAsync(answerId);
    }

    public Task<TypeInternalExternal?> GetQuestionnaireTypeInternalExternalAsync(int answerId)
    {
        return _answerRepository.GetQuestionnaireTypeInternalExternalAsync(answerId);
    }

    public async Task<List<Answer>> GetAnswersFiliereAsync()
    {
        var answers = await _answerRepository.GetAnswersFiliereAsync();
        return _mapper.Map<List<Answer>>(answers);
    }

    public async Task<List<Answer>> GetAnswersModuleAsync()
    {
        var answers = await _answerRepository.GetAnswersModuleAsync();
        return _mapper.Map<List<Answer>>(answers);
    }

    public async Task<List<Answer>> GetAnswersByQuestionIdAsync(int questionId)
    {
        var answers = await _answerRepository.GetAnswersByQuestionIdAsync(questionId);
        return _mapper.Map<List<Answer>>(answers);
    }

    public async Task<List<Answer>> GetAnswersByQuestionnaireIdAsync(int questionnaireId)
    {
        var answers = await _answerRepository.GetAnswersByQuestionnaireIdAsync(questionnaireId);
        return _mapper.Map<List<Answer>>(answers);
    }

    public async Task<List<Answer>> GetAnswersByRespondentIdAsync(string respondentId)
    {
        var answers = await _answerRepository.GetAnswersByRespondentIdAsync(respondentId);
        return _mapper.Map<List<Answer>>(answers);
    }

    public async Task<List<Answer>> GetAllAnswersAsync()
    {
        var answers = await _answerRepository.GetAllAnswersAsync();   
        return _mapper.Map<List<Answer>>(answers);
    }

    public async Task<Answer> GetAnswerByIdAsync(int answerId)
    {
        var answer = await _answerRepository.GetAnswerByIdAsync(answerId);  
        return _mapper.Map<Answer>(answer);
    }

    public async Task<Answer> DeleteAnswerByIdAsync(int answerId)
    {
        var answer = await _answerRepository.DeleteAnswerByIdAsync(answerId); 
        return _mapper.Map<Answer>(answer);
    }

    public async Task<Answer> UpdateCleanAnswerAsync(Answer updatedAnswer)
    {
        var answer = await _answerRepository.UpdateAnswerAsync(updatedAnswer);
        return _mapper.Map<Answer>(answer);
    }

    public async Task<Answer> AddCleanAnswerAsync(AnswerSubmissionDto answerSubmissionDto)
    {
        List<Answer> answerAsList = await _aiAnalyzer.AddCleanAnswersRangeAsync(new List<AnswerSubmissionDto>(){ answerSubmissionDto });
        return _mapper.Map<Answer>(answerAsList[0]);
    }
    
    // NOT in The Controller
    public async Task<List<Question?>> GetQuestionsOfAnswersList(List<Answer> answers)
    {
        return await _answerRepository.GetQuestionsOfAnswersList(answers);
    }
    
    public async Task<Question> GetQuestionOfAnswerById(int answerId)
    {
        return await _answerRepository.GetQuestionOfAnswerById(answerId);
    }

    public async Task<List<Answer>> DeleteAnswersByRespondentId(string respondentId)
    {
        return await _answerRepository.DeleteAnswersByRespondentId(respondentId);
    }
}

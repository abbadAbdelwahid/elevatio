using AutoMapper;
using EvaluationService.DTOs;
using EvaluationService.Models;
using EvaluationService.Repositories;

namespace EvaluationService.Services;

public class QuestionService : IQuestionService
{
    private readonly IQuestionRepository _questionRepository;
    private readonly IQuestionsStandardRepository _questionsStandardRepository;
    private readonly IMapper _mapper;
    
    public QuestionService(IQuestionRepository questionRepository, IQuestionsStandardRepository questionsStandardRepository, IMapper mapper)
    { 
        _questionRepository = questionRepository;
        _questionsStandardRepository = questionsStandardRepository;
        _mapper = mapper;
    } 
    
    public async Task<List<Question>> AddRangeAsync(List<CreateQuestionDto> questionsDto)
    {
        return await _questionRepository.AddRangeAsync(_mapper.Map<List<Question>>(questionsDto));;
    }

    public async Task<List<Question>> GetQuestionsByQuestionnaireIdAsync(int questionnaireId)
    {
        return await _questionRepository.GetQuestionsByQuestionnaireIdAsync(questionnaireId); 
    }

    public async Task<List<StandardQuestion>> GetStandardQuestionsAsync(int questionnaireId)
    {
        return await _questionRepository.GetStandardQuestionsAsync();
    }

    public async Task<Question> AddQuestionAsync(CreateQuestionDto questionDto)
    {
        return await _questionRepository.AddQuestionAsync(_mapper.Map<Question>(questionDto));
    }

    public async Task<Question> GetQuestionAsync(int questionId)
    {
        return await _questionRepository.GetQuestionAsync(questionId);
    }

    public async Task<Question> DeleteQuestionAsync(int questionId)
    {
        return await _questionRepository.DeleteQuestionAsync(questionId);
    }

    public async Task<Question> UpdateQuestionAsync(Question updatedQuestion)
    {
        return await _questionRepository.UpdateQuestionAsync(updatedQuestion);
    }

    public async Task<List<Question>> GetAllQuestionsAsync()
    {
        return await _questionRepository.GetAllQuestionsAsync();
    }

    public async Task<List<Question>> DeleteRangeAsync(List<Question> questions)
    {
        var deletedQuestions = await _questionRepository.DeleteRangeAsync(questions);
        return deletedQuestions;
    }
}
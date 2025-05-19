using AutoMapper;
using EvaluationService.DTOs;
using EvaluationService.Models;
using EvaluationService.Repositories;

namespace EvaluationService.Services;

public class QuestionsStandardService : IQuestionsStandardService
{
    private readonly IQuestionsStandardRepository _questionsStandardRepository;
    private readonly IMapper _mapper;
    
    public QuestionsStandardService(IQuestionsStandardRepository questionsStandardRepository, IMapper mapper)
    {
        _questionsStandardRepository = questionsStandardRepository;
        _mapper = mapper;
    }
    
    public async Task<List<StandardQuestion>> GetQuestionsStandardsByStatName(StatName statName)
    {
        return await _questionsStandardRepository.GetQuestionsStandardsByStatName(statName);
    }

    public async Task<StandardQuestion> GetStandardQuestionById(int id)
    {
        return await _questionsStandardRepository.GetStandardQuestionById(id);
    }

    public async Task<List<StandardQuestion>> GetStandardQuestions()
    {
        return await _questionsStandardRepository.GetStandardQuestions();
    }

    public async Task<StandardQuestion> DeleteStandardQuestionById(int id)
    {
        return await _questionsStandardRepository.DeleteStandardQuestionById(id);
    }

    public async Task<List<StandardQuestion>> DeleteStandardQuestionsByStatName(StatName statName)
    {
        return await _questionsStandardRepository.DeleteStandardQuestionsByStatName(statName);
    }

    public async Task<StandardQuestion> AddStandardQuestion(CreateStandardQuestionDto standardQuestionDto)
    {
        return await _questionsStandardRepository.AddStandardQuestion(_mapper.Map<StandardQuestion>(standardQuestionDto));
    }

    public async Task<StandardQuestion> UpdateStandardQuestion(CreateStandardQuestionDto updateStandardQuestionDto)
    {
        return await _questionsStandardRepository.UpdateStandardQuestion(_mapper.Map<StandardQuestion>(updateStandardQuestionDto));
    }
}
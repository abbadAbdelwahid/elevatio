using AutoMapper;
using EvaluationService.Models;

namespace EvaluationService.DTOs;

public class MappingConfig
{
    public static MapperConfiguration RegisterMappings()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<AnswerSubmissionDto, Answer>();
            config.CreateMap<Answer, AnswerSubmissionDto>();
            config.CreateMap<CreateQuestionDto, Question>();
            config.CreateMap<Question, CreateQuestionDto>();
            config.CreateMap<CreateEvaluationDto, Evaluation>();
            config.CreateMap<Evaluation, CreateEvaluationDto>();
            config.CreateMap<CreateQuestionnaireDto, Questionnaire>();
            config.CreateMap<Questionnaire, CreateQuestionnaireDto>();
            config.CreateMap<CreateStandardQuestionDto, StandardQuestion>();
            config.CreateMap<StandardQuestion, CreateStandardQuestionDto>();
            config.CreateMap<AnswerResponseDto, Answer>();
            config.CreateMap<Answer, AnswerResponseDto>();
        });
        return mappingConfig;
    }
}
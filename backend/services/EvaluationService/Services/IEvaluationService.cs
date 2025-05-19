using EvaluationService.DTOs;
using EvaluationService.Models;

namespace EvaluationService.Services;

public interface IEvaluationService
{
    Task HandlingCascadeDeletion();
    Task<List<Evaluation>> AddRangeAsync(List<EvaluationDto> evaluationsDto);
    Task<string> GetEvaluationType(int evaluationId);
    Task<List<Evaluation>> GetEvaluationsByFiliereIdAsync(int filiereId);
    Task<List<Evaluation>> GetEvaluationsByModuleIdAsync(int moduleId);
    Task<List<Evaluation>> GetEvaluationsByRespondentIdAsync(string respondentId);
    Task<List<Evaluation>> GetAllEvaluationsAsync();
    Task<Evaluation> GetEvaluationByIdAsync(int evaluationId);
    Task<Evaluation> DeleteEvaluationByIdAsync(int evaluationId);
    Task<Evaluation> UpdateEvaluationAsync(EvaluationDto evaluationDto);
    Task<Evaluation> AddEvaluationAsync(EvaluationDto evaluationDto);
}
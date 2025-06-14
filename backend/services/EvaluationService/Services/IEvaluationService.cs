using EvaluationService.DTOs;
using EvaluationService.Models;

namespace EvaluationService.Services;

public interface IEvaluationService
{
    Task HandlingCascadeDeletion();
    Task<List<Evaluation>> AddRangeAsync(List<CreateEvaluationDto> evaluationsDto);
    Task<TypeModuleFiliere?> GetEvaluationType(int evaluationId);
    Task<List<Evaluation>> GetAllEvaluationsFiliereAsync();
    Task<List<Evaluation>> GetEvaluationsModuleAsync();
    Task<List<Evaluation>> GetEvaluationsByFiliereIdAsync(int filiereId);
    Task<List<Evaluation>> GetEvaluationsByModuleIdAsync(int moduleId);
    Task<List<Evaluation>> GetEvaluationsByRespondentIdAsync(string respondentId);
    Task<List<Evaluation>> GetAllEvaluationsAsync();
    Task<Evaluation> GetEvaluationByIdAsync(int evaluationId);
    Task<Evaluation> DeleteEvaluationByIdAsync(int evaluationId);
    Task<Evaluation> UpdateEvaluationAsync(Evaluation evaluationUpdateDto);
    Task<Evaluation> AddEvaluationAsync(CreateEvaluationDto createEvaluationDto);
    Task<List<Evaluation>> DeleteEvaluationsByRespondentIdAsync(string respondentId);
    Task<List<Evaluation>> DeleteEvaluationsByFiliereIdAsync(int filiereId);
    Task<List<Evaluation>> DeleteEvaluationsByModuleIdAsync(int moduleId);
}
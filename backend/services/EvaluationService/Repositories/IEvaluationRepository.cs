using System.Security.Cryptography.Xml;
using EvaluationService.Models;

namespace EvaluationService.Repositories;

public interface IEvaluationRepository
{
    Task<List<Evaluation>> AddRangeAsync(List<Evaluation> evaluations);
    Task<TypeModuleFiliere?> GetEvaluationType(int evaluationId);
    Task<List<Evaluation>> GetEvaluationsByFiliereIdAsync(int filiereId);
    Task<List<Evaluation>> GetEvaluationsByModuleIdAsync(int moduleId);
    Task<List<Evaluation>> GetEvaluationsByRespondentIdAsync(string respondentId);
    Task<List<Evaluation>> GetEvaluationsFiliereAsync();
    Task<List<Evaluation>> GetEvaluationsModuleAsync();
    Task<List<Evaluation>> GetAllEvaluationsAsync();
    Task<Evaluation> GetEvaluationByIdAsync(int evaluationId);
    Task<Evaluation> DeleteEvaluationByIdAsync(int evaluationId);
    Task<List<Evaluation>> DeleteEvaluationsByRespondentIdAsync(string respondentId);
    Task<List<Evaluation>> DeleteEvaluationsByFiliereIdAsync(int filiereId);
    Task<List<Evaluation>> DeleteEvaluationsByModuleIdAsync(int moduleId);
    Task<Evaluation> UpdateEvaluationAsync(Evaluation evaluation);
    Task<Evaluation> AddEvaluationAsync(Evaluation evaluation);
    Task<List<Evaluation>> DeleteRangeAsync(List<Evaluation> evaluations);
}
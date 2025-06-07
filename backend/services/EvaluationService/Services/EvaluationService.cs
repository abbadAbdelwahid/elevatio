using AutoMapper;
using EvaluationService.Data;
using EvaluationService.DTOs;
using EvaluationService.Models;
using EvaluationService.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace EvaluationService.Services;

public class EvaluationService : IEvaluationService
{
    private readonly IEvaluationRepository _evaluationRepository;
    private readonly IExternalValidationService _externalValidationService;
    private readonly IMapper _mapper;
    
    public EvaluationService(IEvaluationRepository evaluationRepository, IMapper mapper, IExternalValidationService externalValidationService)
    {
        _evaluationRepository = evaluationRepository;
        _externalValidationService = externalValidationService;
        _mapper = mapper;
    }

    public async Task HandlingCascadeDeletion()
    {
        // 1. Récupération unique des IDs pertinents
        var evaluations = await _evaluationRepository.GetAllEvaluationsAsync();

        var filiereIds = evaluations
            .Where(e => e.FiliereId != null)
            .Select(e => e.FiliereId)
            .Distinct()
            .ToList();

        var moduleIds = evaluations
            .Where(e => e.ModuleId != null)
            .Select(e => e.ModuleId)
            .Distinct()
            .ToList();

        var respondentIds = evaluations
            .Where(e => !string.IsNullOrEmpty(e.RespondentUserId))
            .Select(e => e.RespondentUserId)
            .Distinct()
            .ToList();

        try
        {
            // 2. Appels batch aux services d'existence
            var modulesExistMap   = await _externalValidationService.ModulesExistAsync(moduleIds);
            var usersExistMap   = await _externalValidationService.UsersExistAsync(respondentIds);
            var filieresExistMap  = await _externalValidationService.FilieresExistAsync(filiereIds);

            var evaluationsToDelete = new List<Evaluation>();
            
            // 3. Supprimer les évaluations invalides
            foreach (var evaluation in evaluations)
            {
                var isFiliereValid = evaluation.FiliereId == null || filieresExistMap.GetValueOrDefault(evaluation.FiliereId.Value, true);
                var isModuleValid  = evaluation.ModuleId == null  || modulesExistMap.GetValueOrDefault(evaluation.ModuleId.Value, true);
                var isUserValid    = string.IsNullOrEmpty(evaluation.RespondentUserId) ||
                                     usersExistMap.GetValueOrDefault(evaluation.RespondentUserId, true);

                if (!isFiliereValid || !isModuleValid || !isUserValid)
                {
                    evaluationsToDelete.Add(evaluation);
                }
            }
            await _evaluationRepository.DeleteRangeAsync(evaluationsToDelete);
        }
        catch (Exception e)
        {
            Console.WriteLine($"[HandlingCascadeDeletion] {e.Message}");
        }
    }
    
    public async Task<List<Evaluation>> AddRangeAsync(List<CreateEvaluationDto> evaluationsDto)
    {
        return await _evaluationRepository.AddRangeAsync(_mapper.Map<List<Evaluation>>(evaluationsDto));
    }

    public async Task<TypeModuleFiliere?> GetEvaluationType(int evaluationId)
    {
        // await HandlingCascadeDeletion();
        var evaluation = await _evaluationRepository.GetEvaluationByIdAsync(evaluationId);
        return await _evaluationRepository.GetEvaluationType(evaluationId);
    }

    public async Task<List<Evaluation>> GetAllEvaluationsFiliereAsync()
    {
        return await _evaluationRepository.GetEvaluationsFiliereAsync();
    }

    public async Task<List<Evaluation>> GetEvaluationsModuleAsync()
    {
        return await _evaluationRepository.GetEvaluationsModuleAsync();
    }

    public async Task<List<Evaluation>> GetEvaluationsByFiliereIdAsync(int filiereId)
    {
        // await HandlingCascadeDeletion();
        return await _evaluationRepository.GetEvaluationsByFiliereIdAsync(filiereId);
    }

    public async Task<List<Evaluation>> GetEvaluationsByModuleIdAsync(int moduleId)
    {
        // await HandlingCascadeDeletion();
        return await _evaluationRepository.GetEvaluationsByModuleIdAsync(moduleId);   
    }

    public async Task<List<Evaluation>> GetEvaluationsByRespondentIdAsync(string respondentId)
    {
        // await HandlingCascadeDeletion();
        return await _evaluationRepository.GetEvaluationsByRespondentIdAsync(respondentId);
    }

    public async Task<List<Evaluation>> GetAllEvaluationsAsync()
    {
        // await HandlingCascadeDeletion();
        return await _evaluationRepository.GetAllEvaluationsAsync();  
    }

    public async Task<Evaluation> GetEvaluationByIdAsync(int evaluationId)
    {
        // await HandlingCascadeDeletion();
        return await _evaluationRepository.GetEvaluationByIdAsync(evaluationId);
    }

    public async Task<Evaluation> DeleteEvaluationByIdAsync(int evaluationId)
    {
        // await HandlingCascadeDeletion();
        return await _evaluationRepository.DeleteEvaluationByIdAsync(evaluationId);
    }

    public async Task<Evaluation> UpdateEvaluationAsync(Evaluation evaluationUpdateDto)
    {
        // await HandlingCascadeDeletion();
        return await _evaluationRepository.UpdateEvaluationAsync(_mapper.Map<Evaluation>(evaluationUpdateDto));
    }

    public async Task<Evaluation> AddEvaluationAsync(CreateEvaluationDto createEvaluationDto)
    {
        // await HandlingCascadeDeletion();
        return await _evaluationRepository.AddEvaluationAsync(_mapper.Map<Evaluation>(createEvaluationDto));   
    }

    public async Task<List<Evaluation>> DeleteEvaluationsByRespondentIdAsync(string respondentId)
    {
        // await HandlingCascadeDeletion();
        return await _evaluationRepository.DeleteEvaluationsByRespondentIdAsync(respondentId);
    }

    public async Task<List<Evaluation>> DeleteEvaluationsByFiliereIdAsync(int filiereId)
    {
        // await HandlingCascadeDeletion();
        return await _evaluationRepository.DeleteEvaluationsByFiliereIdAsync(filiereId);
    }

    public async Task<List<Evaluation>> DeleteEvaluationsByModuleIdAsync(int moduleId)
    {
        // await HandlingCascadeDeletion();
        return await _evaluationRepository.DeleteEvaluationsByModuleIdAsync(moduleId);
    }
}
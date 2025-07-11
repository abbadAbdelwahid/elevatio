using System.ComponentModel.DataAnnotations;
using EvaluationService.Models;

namespace EvaluationService.DTOs;

public class CreateEvaluationDto
{
    [Required]
    public string? RespondentUserId { get; set; } 
    
    public string? Comment { get; set; }

    [Required]
    public TypeModuleFiliere Type { get; set; }
    
    public int? FiliereId { get; set; }
    public int? ModuleId { get; set; }

    /// Score AI 1–5 ou -1 pour les réponses illogiques ou vides
    [Required]
    public float Score { get; set; }
}
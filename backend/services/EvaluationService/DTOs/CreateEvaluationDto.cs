using System.ComponentModel.DataAnnotations;

namespace EvaluationService.DTOs;

public class CreateEvaluationDto
{
    [Required]
    public int RespondentUserId { get; set; }

    public int? FiliereId { get; set; }
    public int? ModuleId { get; set; }

    /// Score AI 1–5 ou -1 pour les réponses illogiques ou vides
    [Required]
    public float Score { get; set; }
}
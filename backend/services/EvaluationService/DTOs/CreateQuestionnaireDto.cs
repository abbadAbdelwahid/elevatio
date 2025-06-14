using System.ComponentModel.DataAnnotations;
using EvaluationService.Models;

namespace EvaluationService.DTOs;

public class CreateQuestionnaireDto
{
    [Required]
    [MaxLength(100)]
    public string? Title { get; set; }

    [Required]
    public TypeInternalExternal TypeInternalExternal { get; set; }
    
    [Required]
    public TypeModuleFiliere TypeModuleFiliere { get; set; }

    public int? FiliereId { get; set; }

    public int? ModuleId { get; set; }

    [Required]
    public string? CreatorUserId { get; set; }  // Validé via Auth MS
    
    public ICollection<CreateQuestionWithQuestionnaire>? Questions { get; set; }

}
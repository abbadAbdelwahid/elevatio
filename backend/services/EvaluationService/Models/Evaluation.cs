using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvaluationService.Models;

public class Evaluation
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int EvaluationId { get; set; }

    [Required, MaxLength(100)]
    public string? RespondentUserId { get; set; }
    
    [Required]
    public TypeModuleFiliere Type { get; set; }

    [MaxLength(1000)]
    public string? Comment { get; set; }
    
    public int? FiliereId { get; set; }
    public int? ModuleId { get; set; }

    /// Score AI 1–5 ou -1 pour les réponses illogiques ou vides
    [Required]
    public float Score { get; set; }

    public DateTime EvaluatedAt { get; set; } = DateTime.UtcNow;
}
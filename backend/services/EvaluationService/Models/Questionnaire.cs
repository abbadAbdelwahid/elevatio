using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace EvaluationService.Models;

public class Questionnaire
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int QuestionnaireId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Title { get; set; }

    [Required]
    public TypeInternalExternal TypeInternalExternal { get; set; }

    public TypeModuleFiliere TypeModuleFiliere { get; set; }
    
    /// FK vers filière (si questionnaire de filière)
    public int? FiliereId { get; set; }

    /// FK vers module (si questionnaire de module)
    public int? ModuleId { get; set; }

    [Required, MaxLength(100)]
    public string? CreatorUserId { get; set; }  // Validé via Auth MS

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // [JsonIgnore]
    public ICollection<Question>? Questions { get; set; }
}
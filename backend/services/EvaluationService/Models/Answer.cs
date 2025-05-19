using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EvaluationService.Models;

public class Answer
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int AnswerId { get; set; }

    [Required]
    [ForeignKey(nameof(Question))]
    public int QuestionId { get; set; }
    
    [JsonIgnore]
    public Question? Question { get; set; }

    // [Required]
    // public int RespondentUserId { get; set; } // Étudiant ou évaluateur externe

    // public int? FiliereId { get; set; }
    // public int? ModuleId  { get; set; }

    [MaxLength(2000)]
    public string? RawAnswer { get; set; }
    
    [Range(1,5)]
    public float? RatingAnswer { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EvaluationService.Models;

public class Question
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int QuestionId { get; set; }

    [ForeignKey(nameof(Questionnaire))]
    public int QuestionnaireId { get; set; }
    
    [JsonIgnore]
    public Questionnaire? Questionnaire { get; set; }

    /// Si question standard, référence StandardQuestionId
    public int? StandardQuestionId { get; set; }

    [Required, MaxLength(500)]
    public string Text { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [JsonIgnore]
    public ICollection<Answer>? Answers { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace EvaluationService.DTOs;

public class CreateQuestionDto
{   public int Id {get; set;} 
    public int QuestionnaireId { get; set; }

    /// Si question standard, référence StandardQuestionId
    public string StatName { get; set; }

    [Required, MaxLength(500)]
    public string QuestionText { get; set; }
}
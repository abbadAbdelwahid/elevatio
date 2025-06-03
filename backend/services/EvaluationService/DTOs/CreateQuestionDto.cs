using System.ComponentModel.DataAnnotations;

namespace EvaluationService.DTOs;

public class CreateQuestionDto
{
    public int QuestionnaireId { get; set; }

    /// Si question standard, référence StandardQuestionId
    public int? StandardQuestionId { get; set; }

    [Required, MaxLength(500)]
    public string Text { get; set; }
}
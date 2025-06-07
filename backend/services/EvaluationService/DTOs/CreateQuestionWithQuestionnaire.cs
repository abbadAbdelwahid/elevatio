using System.ComponentModel.DataAnnotations;

namespace EvaluationService.DTOs;

public class CreateQuestionWithQuestionnaire
{
    public int? StandardQuestionId { get; set; }

    [MaxLength(500)]
    public string? Text { get; set; }
}
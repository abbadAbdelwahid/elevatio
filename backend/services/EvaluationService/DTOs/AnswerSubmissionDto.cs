using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvaluationService.DTOs;

public class AnswerSubmissionDto
{
    [Required]
    public int QuestionId { get; set; }

    // [Required]
    // public int RespondentUserId { get; set; }

    [MaxLength(2000)]
    public string? AnswerText { get; set; }
    
    [Range(1,5)]
    public int? AnswerValue { get; set; }
}
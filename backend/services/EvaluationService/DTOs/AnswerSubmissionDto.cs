using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvaluationService.DTOs;

public class AnswerSubmissionDto
{
    [Required]
    public int QuestionId { get; set; }

    [Required]
    public string RespondentUserId { get; set; }

    [MaxLength(2000)]
    public string? RawAnswer { get; set; }
    
    [Range(1,5)]
    public float? RatingAnswer { get; set; }
}
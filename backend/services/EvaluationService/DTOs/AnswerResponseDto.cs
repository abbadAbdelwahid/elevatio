using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EvaluationService.Models;

namespace EvaluationService.DTOs;

public class AnswerResponseDto
{
    public int AnswerId { get; set; }

    public int QuestionId { get; set; }

    public string? RawAnswer { get; set; }
    
    public float? RatingAnswer { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

}
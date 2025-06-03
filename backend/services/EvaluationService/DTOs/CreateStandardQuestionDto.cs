using System.ComponentModel.DataAnnotations;
using EvaluationService.Models;

namespace EvaluationService.DTOs;

public class CreateStandardQuestionDto
{
    
    [Required, MaxLength(500)]
    public string Text { get; set; }

    [Required]
    public StatName StatName { get; set; }
}
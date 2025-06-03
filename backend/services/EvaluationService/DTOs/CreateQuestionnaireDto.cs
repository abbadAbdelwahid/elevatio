using System.ComponentModel.DataAnnotations;
using EvaluationService.Models;

namespace EvaluationService.DTOs;

public class CreateQuestionnaireDto
{
    [Required]
    [MaxLength(100)]
    public string Title { get; set; }

    [Required]
    public QuestionnaireType Type { get; set; }

    public int? FiliereId { get; set; }

    public int? ModuleId { get; set; }


}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvaluationService.Models;

public class StandardQuestion
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int StandardQuestionId { get; set; }

    [Required, MaxLength(500)]
    public string Text { get; set; }

    [Required]
    public StatName StatName { get; set; }
}
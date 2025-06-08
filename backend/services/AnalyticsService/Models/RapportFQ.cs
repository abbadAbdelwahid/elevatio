using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnalyticsService.Models;

public class RapportFQ
{  
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
    public int      Id     { get; set; }  // PK
    public int      QuestionnaireId { get; set; } // FK vers Questionnaire 
    public int FiliereId { get; set; } 
    [Column(TypeName = "bytea")] 
    public byte[]?  RapportPdf { get; set; } 
    public DateTime CreatedAt    { get; set; }
}
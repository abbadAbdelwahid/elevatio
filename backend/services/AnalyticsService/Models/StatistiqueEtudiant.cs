using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnalyticsService.Models;

public class StatistiqueEtudiant
{ 
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; } 
    public int StudentId { get; set; }
    public int? FiliereId { get; set; }
    public double NoteMoyenne { get; set; } 
    public double NoteMax { get; set; }
    public double NoteMin { get; set; }
    public double? PassRate { get; set; }  
    public double? Median { get; set; }
    public DateTime? CreatedAt { get; set; } 
    public String? Observation { get; set; }
    [Column(TypeName = "bytea")] 
    public byte[] RapportPdf { get; set; } 
    
    
}
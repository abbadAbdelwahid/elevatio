using System.ComponentModel.DataAnnotations;

namespace AnalyticsService.Models;
using System.ComponentModel.DataAnnotations.Schema;
public class StatistiqueModule
{   
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
    public int      Id    { get; set; }  // PK
    public int      ModuleId               { get; set; }  // Référence métier
    public double?   AverageRating        { get; set; }  // Moyenne des notes 
    public double?   AverageNotes       { get; set; }  // Moyenne des notes 
    public double?   NoteMax        { get; set; }  // Moyenne des notes 
    public double?   NoteMin        { get; set; }  // Moyenne des notes 
    public double?   MedianNotes           { get; set; }  // Médiane des notes
    public double?  PassRate       { get; set; }                                        
    public double?  StdevNotes           { get; set; }  // Écart-type
    public double?  ParticipationRate      { get; set; }   // % de retours 
    public double?  SatisfactionRate      { get; set; }
    public double?  NpsScore               { get; set; }  // Net Promoter Score
    public double?   PositiveFeedbackPct    { get; set; }  // % positifs
    public double?  NegativeFeedbackPct    { get; set; }  // % négatifs
  
    public DateTime? CreatedAt              { get; set; }  // Date de génération   
    public String? Rapport       { get; set; } 
    [Column(TypeName = "bytea")] 
    public byte[]? RapportPdf { get; set; } 
}
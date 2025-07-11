using System.ComponentModel.DataAnnotations;

namespace AnalyticsService.Models;
using System.ComponentModel.DataAnnotations.Schema;
public class StatistiqueEnseignant
{ 
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
    public int      Id{ get; set; }  // PK
    public int      TeacherId             { get; set; }  // Référence métier
    public double?  AverageM     { get; set; } // Moyenne des modules 
    public double?  MedianNotes         { get; set; }  // Médiane des moyennes 
    public double?  StdEv     { get; set; }  
    public String? ModuleMaxPass          { get; set; }  
    public String? ModuleMinPass          { get; set; }  
    public String? MaxModuleRated        { get; set; }  
    public double?  NoteMax      { get; set; } 
    public double?  NoteMin        { get; set; }  // Moyenne des notes 
    public double?  PassRate       { get; set; }  
    public double?  PositiveFeedBackPct    { get; set; }    
    public double?  NegativeFeedBackPct    { get; set; }

    public DateTime? CreatedAt             { get; set; }  // Date de génération 
    public String? Rapport  { get; set; } 
    [Column(TypeName = "bytea")] 
    public byte[]? RapportPdf { get; set; } 
}
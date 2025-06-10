using System.ComponentModel.DataAnnotations;

namespace AnalyticsService.Models;
using System.ComponentModel.DataAnnotations.Schema;
public class StatistiqueFiliere
{ 
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
    public int      Id   { get; set; }  // PK 
    public int      FiliereId              { get; set; } // Référence métier
    public String FiliereName               { get; set; }
    public String? FiliereRoot               { get; set; }
    public int?     NbrEtds               { get; set; } 
    public double?   AverageRating          { get; set; } // Moyenne des notes
    public double?   AverageMoyenne          { get; set; } // Moyenne des notes 
    public String? ModuleMaxPass          { get; set; }  
    public String? ModuleMinPass          { get; set; } 
    public String? Majorant               { get; set; }
    public double? MaxMoyenne               { get; set; }  
    public String? MaxModuleRated { get; set; }
    

    public double?   MedianRating           { get; set; }  // Médiane des notes
    public double?  StdDevRating           { get; set; }  // Écart-type des notes
    public double?  SatisfactionRate       { get; set; }  // % notes ≥ seuil 
    public double?  NpsScore               { get; set; }  // Net Promoter Score
    public double?  PositiveFeedbackPct    { get; set; }  // % feedbacks positifs
    public double?  NegativeFeedbackPct    { get; set; }  // % feedbacks négatifs 
    public double?  PassRate       { get; set; }
    public DateTime CreatedAt              { get; set; }  // Date de génération 
    [Column(TypeName = "bytea")] 
    public byte[]?  RapportPdf { get; set; } 
}
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
    public double?   MedianRating           { get; set; }  // Médiane des notes
    public double?  StdDevRating           { get; set; }  // Écart-type
    public double?  ParticipationRate      { get; set; }  // % de retours
    public double?  CompletionTimeAvg      { get; set; }  // Durée moyenne (min)
    public double?  DropoutRate            { get; set; }  // % d’abandon
    public double?  NpsScore               { get; set; }  // Net Promoter Score
    public int?      CommentCount           { get; set; }  // Nb de commentaires
    public double?   PositiveFeedbackPct    { get; set; }  // % positifs
    public double?  NegativeFeedbackPct    { get; set; }  // % négatifs
    public double?   ImprovementTrend       { get; set; }  // Δ moyenne entre sessions
    public DateTime CreatedAt              { get; set; }  // Date de génération   
    public String Rapport       { get; set; } 
    [Column(TypeName = "bytea")] 
    public byte[] RapportPdf { get; set; } 
}
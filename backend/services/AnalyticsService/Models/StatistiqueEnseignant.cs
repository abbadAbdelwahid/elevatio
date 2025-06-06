using System.ComponentModel.DataAnnotations;

namespace AnalyticsService.Models;
using System.ComponentModel.DataAnnotations.Schema;
public class StatistiqueEnseignant
{ 
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
    public int      Id{ get; set; }  // PK
    public int      TeacherId             { get; set; }  // Référence métier
    public double   AverageRating         { get; set; }  // Moyenne des notes 
    public double   AverageRatingM     { get; set; } // Moyenne des modules 
    public double   MedianRating          { get; set; }  // Médiane des notes
    public double   StdDevRating          { get; set; }  // Écart-type
    public double   ParticipationRate     { get; set; }  // % de retours
    public double   NpsScore              { get; set; }  // Net Promoter Score
    public double   PositiveFeedbackPct   { get; set; }  // % positifs
    public double   NegativeFeedbackPct   { get; set; }  // % négatifs
    public double   PeerReviewScore       { get; set; }  // Évaluation 360°
    public double   ResponseTimeAvg       { get; set; }  // Temps moyen de réponse
    public double   ImprovementTrend      { get; set; }  // Δ moyenne entre sessions
    public DateTime CreatedAt             { get; set; }  // Date de génération 
    public String Rapport  { get; set; }
    [Column(TypeName = "bytea")] 
    public byte[] RapportPdf { get; set; } 
}
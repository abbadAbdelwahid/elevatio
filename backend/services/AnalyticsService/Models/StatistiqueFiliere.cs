namespace AnalyticsService.Models;
using System.ComponentModel.DataAnnotations.Schema;
public class StatistiqueFiliere
{
    public int      Id   { get; set; }  // PK
    public int      FiliereId              { get; set; }  // Référence métier
    public double   AverageRating          { get; set; }  // Moyenne des notes
    public double   MedianRating           { get; set; }  // Médiane des notes
    public double   StdDevRating           { get; set; }  // Écart-type des notes
    public double   SatisfactionRate       { get; set; }  // % notes ≥ seuil
    public double   ParticipationRate      { get; set; }  // % de retours
    public double   NpsScore               { get; set; }  // Net Promoter Score
    public double   PositiveFeedbackPct    { get; set; }  // % feedbacks positifs
    public double   NegativeFeedbackPct    { get; set; }  // % feedbacks négatifs
    public int      ActionPlanCount        { get; set; }  // Nb d’actions déclenchées
    public double   ImprovementTrend       { get; set; }  // Δ moyenne vs session précédente
    public DateTime CreatedAt              { get; set; }  // Date de génération 
    [Column(TypeName = "bytea")] 
    public byte[] RapportPdf { get; set; } 
}
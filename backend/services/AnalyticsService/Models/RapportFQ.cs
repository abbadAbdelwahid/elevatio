namespace AnalyticsService.Models;

public class RapportFQ
{
    public int      Id     { get; set; }  // PK
    public int      QuestionnaireId { get; set; }  // FK vers Questionnaire
    public string   Data      { get; set; }
    public DateTime GeneratedAt     { get; set; }
}
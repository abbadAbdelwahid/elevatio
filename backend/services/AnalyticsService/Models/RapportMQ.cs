namespace AnalyticsService.Models;

public class RapportMQ
{
    public int      Id     { get; set; }  // PK
    public int QuestionnaireId { get; set; } // FK vers Questionnaire 
    public int ModuleId { get; set; }
    public String   Data      { get; set; }
    public DateTime GeneratedAt     { get; set; }

}
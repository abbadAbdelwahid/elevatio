namespace AnalyticsService.ExternalClients.DTO;

public class NoteDto
{
    public int NoteId { get; set; }  // Clé primaire

    public int ModuleId { get; set; }  // Clé étrangère vers le module 
    public int studentId { get; set; }
                                       
    public float Grade { get; set; } 
    public string Observation { get; set; }
}
namespace AnalyticsService.ExternalClients.DTO;

public class QuestionnaireDto
{
      public int      Id { get; set; }   // PK
        public int?     ModuleId        { get; set; }   // si c’est un questionnaire de module
        public int?     FiliereId       { get; set; }   // si c’est un questionnaire de filière
        public string   Title           { get; set; }   
        public string   Type            { get; set; }   // “internal” or “external”
        public DateTime CreatedAt       { get; set; }

}
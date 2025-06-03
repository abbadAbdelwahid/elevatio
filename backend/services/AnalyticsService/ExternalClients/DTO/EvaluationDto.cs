namespace AnalyticsService.ExternalClients.DTO;

public class EvaluationDto
{
        public int      Id                   { get; set; }    // PK
        public Guid?    SessionId            { get; set; }    // anonymisation étudiant
        public int?     ExternalEvaluatorId  { get; set; }    // null si étudiant
        public int?     ModuleId             { get; set; }    // Évaluation de module
        public int?     FiliereId            { get; set; }    // Évaluation de filière
        
        public int      Rating               { get; set; } 
        public DateTime CreatedAt            { get; set; }
}
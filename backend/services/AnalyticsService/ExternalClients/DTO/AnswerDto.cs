namespace AnalyticsService.ExternalClients.DTO;

public class AnswerDto
{
    public int      Id             { get; set; }    // PK
    public int      QuestionId           { get; set; }    // FK vers Question
    public int?     ExternalEvaluatorId  { get; set; }    // null si c'est un étudiant
    public Guid?    SessionId            { get; set; }    // identifie anonymement un étudiant
    public int AnswerValue            { get; set; }
    public string?   AnswerText          { get; set; }
    public DateTime? CreatedAt            { get; set; }
}
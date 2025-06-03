namespace AnalyticsService.ExternalClients.DTO;

public class QuestionDto
{
    public int      Id       { get; set; }    // PK
    public int      QuestionnaireId  { get; set; }    // FK vers Questionnaire
    public string   QuestionText    { get; set; }    // Texte de la question
    public string   QuestionType     { get; set; }    // ex. "MultipleChoice", "Scale", "OpenText"
    public string?   StatName         { get; set; }    // Nom de la statistique liée pour l'agrégation
    public DateTime CreatedAt        { get; set; }    // Date de création
}
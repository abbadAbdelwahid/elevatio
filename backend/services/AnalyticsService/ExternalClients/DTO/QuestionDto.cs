using System.Text.Json;
using System.Text.Json.Serialization;

namespace AnalyticsService.ExternalClients.DTO;

public class QuestionDto 
{
    public int      QuestionId       { get; set; }    // PK
    public int      QuestionnaireId  { get; set; }    // FK vers Questionnaire
    public string   QuestionText    { get; set; }    // Texte de la question
    public string?   QuestionType     { get; set; }    // ex. "MultipleChoice", "Scale", "OpenText"
    public String?   StatName         { get; set; }    // Nom de la statistique liée pour l'agrégation
    public DateTime CreatedAt        { get; set; }    // Date de création 
    [JsonExtensionData]
    public Dictionary<string, JsonElement> ExtraData { get; set; }
    // → Mais si je ne veux pas du tout les conserver, je n’ajoute aucun JsonExtensionData.
}
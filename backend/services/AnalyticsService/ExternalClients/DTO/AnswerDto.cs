using System.Text.Json;
using System.Text.Json.Serialization;

namespace AnalyticsService.ExternalClients.DTO;

public class AnswerDto
{
    public int      AnswerId             { get; set; }    // PK
    public int      QuestionId           { get; set; }    // FK vers Question
    public int?     ExternalEvaluatorId  { get; set; }    // null si c'est un étudiant
    public Guid?    SessionId            { get; set; }    // identifie anonymement un étudiant
    public float? RatingAnswer        { get; set; }
    public string?   RawAnswer          { get; set; }
    public DateTime? CreatedAt            { get; set; } 
    [JsonExtensionData]
    public Dictionary<string, JsonElement> ExtraData { get; set; }
    // → Mais si je ne veux pas du tout les conserver, je n’ajoute aucun JsonExtensionData.
}
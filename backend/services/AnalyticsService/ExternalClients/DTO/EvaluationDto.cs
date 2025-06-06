using System.Text.Json;
using System.Text.Json.Serialization;

namespace AnalyticsService.ExternalClients.DTO;

public class EvaluationDto
{
        public int      EvaluationId                   { get; set; }    // PK
        public int RespondentUserId { get; set; }
        public int?     ModuleId             { get; set; }    // Évaluation de module
        public int?     FiliereId            { get; set; }    // Évaluation de filière
        
        public float      Score                { get; set; } 
        public DateTime CreatedAt            { get; set; } 
        [JsonExtensionData]
        public Dictionary<string, JsonElement> ExtraData { get; set; }
        // → Mais si je ne veux pas du tout les conserver, je n’ajoute aucun JsonExtensionData.
}
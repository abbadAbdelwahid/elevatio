using System.Text.Json;

namespace AnalyticsService.ExternalClients.DTO;

using Newtonsoft.Json;  
public class QuestionnaireDto
{ 
      public int      QuestionnaireId { get; set; }   // PK
        public int?     ModuleId        { get; set; }   // si c’est un questionnaire de module
        public int?     FiliereId       { get; set; }   // si c’est un questionnaire de filière
        public string   Title           { get; set; }   
        public string   Type            { get; set; }   // “internal” or “external”
        public DateTime CreatedAt       { get; set; } 
        [JsonExtensionData]
        public Dictionary<string, JsonElement> ExtraData { get; set; }
        // → Mais si je ne veux pas du tout les conserver, je n’ajoute aucun JsonExtensionData.

}
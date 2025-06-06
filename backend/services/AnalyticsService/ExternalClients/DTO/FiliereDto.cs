using System.Text.Json;
using System.Text.Json.Serialization;

namespace AnalyticsService.ExternalClients.DTO;

public class FiliereDto
{
    public int FiliereId { get; set; } // Primary Key
    public string FiliereName { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    [JsonExtensionData]
    public Dictionary<string, JsonElement> ExtraData { get; set; }
}
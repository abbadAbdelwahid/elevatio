using System.Text.Json;
using System.Text.Json.Serialization;

namespace AnalyticsService.ExternalClients.DTO;

public class ModuleDto
{ public int ModuleId { get; set; }  // ID du modulee
    public string ModuleName { get; set; }  // Nom du module
    public string ModuleDescription { get; set; }  // Description du module
    public int ModuleDuration { get; set; }  // Durée du module (en heures)
    public string FiliereName { get; set; }  // Nom de la filière associée
    public int TeacherId { get; set; }  // ID de l'enseignant 
    public string TeacherFullName { get; set; }
    public DateTime CreatedAt { get; set; }  // Date de création
    public DateTime UpdatedAt { get; set; }  // Date de mise à jour 
    public bool Evaluated { get; set; }
    public string ProfileImageUrl { get; set; }
    [JsonExtensionData]
    public Dictionary<string, JsonElement> ExtraData { get; set; }
}
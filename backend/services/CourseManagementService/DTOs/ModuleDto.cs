// /DTOs/ModuleDto.cs
namespace CourseManagementService.DTOs
{
    public class ModuleDto
    {
        public int ModuleId { get; set; }  // ID du module
        public string ModuleName { get; set; }  // Nom du module
        public string ModuleDescription { get; set; }  // Description du module
        public int ModuleDuration { get; set; }  // Durée du module (en heures)
        public string FiliereName { get; set; }  // Nom de la filière associée
        public int FiliereId { get; set; }  // Nom de la filière associée
        public int TeacherId { get; set; }  // ID de l'enseignant
        
        public string TeacherFullName { get; set; }

        public DateTime CreatedAt { get; set; }  // Date de création
        public DateTime UpdatedAt { get; set; }  // Date de mise à jour
    
        public bool Evaluated { get; set; }  // Ajout de la propriété Evaluated

        public string? ProfileImageUrl { get; set; }

    }
}
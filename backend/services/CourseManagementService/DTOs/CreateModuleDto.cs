// /DTOs/CreateModuleDto.cs

using System.ComponentModel.DataAnnotations;

namespace CourseManagementService.DTOs
{
    public class CreateModuleDto
    {
        [Required(ErrorMessage = "Le nom du module est obligatoire.")]
        [MaxLength(50, ErrorMessage = "Le nom du module ne doit pas dépasser 50 caractères.")]
        public string ModuleName { get; set; }
        
        [MaxLength(500, ErrorMessage = "La description ne doit pas dépasser 500 caractères.")]
        public string ModuleDescription { get; set; }
        public int ModuleDuration { get; set; } // Durée en heures
        public string FiliereName { get; set; }  // Nom de la filière
        public int TeacherId { get; set; }  // ID de l'enseignant (assumé déjà fourni)
    }
}
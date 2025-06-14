// /DTOs/UpdateFiliereDto.cs

using System.ComponentModel.DataAnnotations;

namespace CourseManagementService.DTOs
{
    public class UpdateFiliereDto
    {
        [Required(ErrorMessage = "Le nom de la filière est obligatoire.")]
        [MaxLength(50, ErrorMessage = "Le nom de la filière ne doit pas dépasser 50 caractères.")]

        public string FiliereName { get; set; }
        
        [MaxLength(500, ErrorMessage = "La description ne doit pas dépasser 500 caractères.")]

        public string Description { get; set; }
    }
}
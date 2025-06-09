// /DTOs/UpdateNoteDto.cs

using System.ComponentModel.DataAnnotations;

namespace CourseManagementService.DTOs
{
    public class UpdateNoteDto
    {
        [Range(0, 20, ErrorMessage = "La note doit être comprise entre 0 et 20.")]
        public int Grade { get; set; }

        [MaxLength(500)]
        public string Observation { get; set; }
        
    }
}
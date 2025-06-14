// /DTOs/NoteDto.cs
namespace CourseManagementService.DTOs
{
    public class NoteDto
    {
        public int NoteId { get; set; }
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public int StudentId { get; set; }
        
        public string StudentFullName { get; set; }

        public int Grade { get; set; }
        
        public string Observation { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
    }
}
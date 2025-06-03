// /DTOs/UpdateModuleDto.cs
namespace CourseManagementService.DTOs
{
    public class UpdateModuleDto
    {
        public string ModuleName { get; set; }  // Nom du module
        public string ModuleDescription { get; set; }  // Description du module
        public int ModuleDuration { get; set; }  // Durée du module (en heures)
    }
}
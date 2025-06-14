// /Services/Interfaces/IModuleService.cs
using CourseManagementService.DTOs;

namespace CourseManagementService.Services.Interfaces
{
    public interface IModuleService
    {
        Task<ModuleDto> CreateModuleAsync(CreateModuleDto dto);
        Task<ModuleDto?> GetModuleByIdAsync(int id);
        Task<IEnumerable<ModuleDto>> GetAllModulesAsync();
        Task<ModuleDto?> UpdateModuleAsync(int id, UpdateModuleDto dto);
        Task<bool> DeleteModuleAsync(int id);
        Task<IEnumerable<ModuleDto>> GetModulesByFiliereNameAsync(string filiereName);
        public Task<IEnumerable<ModuleDto>> GetFilteredModulesAsync(string filter);
        Task<bool> UpdateModuleImageUrlAsync(int moduleId, string imageUrl); 
        Task<IEnumerable<ModuleDto>> GetModulesByTeacherAsync(int teacherId);

        Task<IEnumerable<UnassignedModuleDto>> GetUnassignedModulesAsync();

        Task<bool> AssignModulesToTeacherAsync(int teacherId, List<int> moduleIds);

        Task<string?> GetTeacherFullNameByModuleIdAsync(int moduleId);

        Task<IEnumerable<ModuleDto>> GetModulesByTeacherIdAsync(int teacherId);

    }
}
using CourseManagementService.DTOs;

namespace CourseManagementService.Services.Interfaces;

public interface IFiliereService
{
    Task<FiliereDto> CreateFiliereAsync(CreateFiliereDto dto);
    Task<IEnumerable<FiliereDto>> GetAllFilieresAsync();
    Task<FiliereDto?> UpdateFiliereAsync(int id, UpdateFiliereDto dto);
    Task<bool> DeleteFiliereAsync(int id);
    Task<FiliereDto?> GetFiliereByIdAsync(int id);
    Task<IEnumerable<FiliereMiniDto>> GetFiliereIdsAndNamesAsync();
    
    Task<bool> FiliereExistsAsync(int filiereId);


    
}
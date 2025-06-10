// /Services/Implementations/ModuleService.cs
using CourseManagementService.DTOs;
using CourseManagementService.ExternalServices;
using CourseManagementService.Models;
using CourseManagementService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementService.Services.Implementations
{
    public class ModuleService : IModuleService
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthHttpClientService _authHttp;


        public ModuleService(ApplicationDbContext context,AuthHttpClientService authHttp)
        {
            _context = context;
            _authHttp = authHttp;

        }

        public async Task<ModuleDto> CreateModuleAsync(CreateModuleDto dto)
        {
            // Cherche l'ID de la filière par son nom
            var filiere = await _context.Filieres
                .FirstOrDefaultAsync(f => f.FiliereName.ToLower() == dto.FiliereName.ToLower());

            if (filiere == null)
                throw new InvalidOperationException("Filière introuvable.");

            // Créer le module
            var module = new Module
            {
                ModuleName = dto.ModuleName,
                ModuleDescription = dto.ModuleDescription,
                ModuleDuration = dto.ModuleDuration,
                FiliereId = filiere.FiliereId, // Lier à la filière par son ID
                TeacherId = dto.TeacherId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Modules.Add(module);
            await _context.SaveChangesAsync();

            return new ModuleDto
            {
                ModuleId = module.ModuleId,
                ModuleName = module.ModuleName,
                ModuleDescription = module.ModuleDescription,
                ModuleDuration = module.ModuleDuration,
                FiliereName = filiere.FiliereName, // Retourner le nom de la filière
                ProfileImageUrl = module.ProfileImageUrl, // 👈 ici
                TeacherId = module.TeacherId,
                CreatedAt = module.CreatedAt,
                UpdatedAt = module.UpdatedAt
            };
        }

        public async Task<ModuleDto?> GetModuleByIdAsync(int id)
        {
            var module = await _context.Modules
                .Include(m => m.Filiere)
                .FirstOrDefaultAsync(m => m.ModuleId == id);

            if (module == null)
                return null;

            var teacherFullName = await _authHttp.GetTeacherFullNameAsync(module.TeacherId);

            return new ModuleDto
            {
                ModuleId = module.ModuleId,
                ModuleName = module.ModuleName,
                ModuleDescription = module.ModuleDescription,
                ModuleDuration = module.ModuleDuration,
                FiliereName = module.Filiere.FiliereName,
                TeacherId = module.TeacherId,
                TeacherFullName = teacherFullName,
                ProfileImageUrl = module.ProfileImageUrl, // 👈 ici
                CreatedAt = module.CreatedAt,
                UpdatedAt = module.UpdatedAt,
                Evaluated = module.Evaluated
            };
        }


        public async Task<IEnumerable<ModuleDto>> GetAllModulesAsync()
        {
            var list = await _context.Modules
                .Include(m => m.Filiere) // Inclure les données de la filière associée
                .AsNoTracking() // Optimiser pour la lecture seule
                .ToListAsync();

            return list.Select(m => new ModuleDto
            {
                ModuleId = m.ModuleId,
                ModuleName = m.ModuleName,
                ModuleDescription = m.ModuleDescription,
                ModuleDuration = m.ModuleDuration,
                FiliereName = m.Filiere.FiliereName, // Inclure le nom de la filière
                ProfileImageUrl = m.ProfileImageUrl, // 👈 ici
                TeacherId = m.TeacherId,
                CreatedAt = m.CreatedAt,
                UpdatedAt = m.UpdatedAt
            });
        }

        public async Task<ModuleDto?> UpdateModuleAsync(int id, UpdateModuleDto dto)
        {
            var module = await _context.Modules
                .Include(m => m.Filiere)  // Inclure la filière associée au module
                .FirstOrDefaultAsync(m => m.ModuleId == id);  // Recherche du module avec l'inclusion de la filière

            if (module == null)
                return null;

            // Mettre à jour les informations du module
            module.ModuleName = dto.ModuleName;
            module.ModuleDescription = dto.ModuleDescription;
            module.ModuleDuration = dto.ModuleDuration;
            module.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new ModuleDto
            {
                ModuleId = module.ModuleId,
                ModuleName = module.ModuleName,
                ModuleDescription = module.ModuleDescription,
                ModuleDuration = module.ModuleDuration,
                FiliereName = module.Filiere.FiliereName,  // Maintenant la filière est chargée
                ProfileImageUrl = module.ProfileImageUrl, // 👈 ici
                TeacherId = module.TeacherId,
                CreatedAt = module.CreatedAt,
                UpdatedAt = module.UpdatedAt
            };
        }


        public async Task<bool> DeleteModuleAsync(int id)
        {
            var module = await _context.Modules.FindAsync(id);
            if (module == null)
                return false;

            _context.Modules.Remove(module);
            await _context.SaveChangesAsync();

            return true;
        }
        
        public async Task<IEnumerable<ModuleDto>> GetModulesByFiliereNameAsync(string filiereName)
        {
            var filiere = await _context.Filieres
                .FirstOrDefaultAsync(f => f.FiliereName.ToLower() == filiereName.ToLower());

            if (filiere == null)
                throw new InvalidOperationException("Filière introuvable.");

            var modules = await _context.Modules
                .Where(m => m.FiliereId == filiere.FiliereId)
                .Include(m => m.Filiere)
                .AsNoTracking()
                .ToListAsync();

            var results = new List<ModuleDto>();

            foreach (var m in modules)
            {
                var fullName = await _authHttp.GetTeacherFullNameAsync(m.TeacherId);

                results.Add(new ModuleDto
                {
                    ModuleId = m.ModuleId,
                    ModuleName = m.ModuleName,
                    ModuleDescription = m.ModuleDescription,
                    ModuleDuration = m.ModuleDuration,
                    FiliereName = m.Filiere.FiliereName,
                    TeacherId = m.TeacherId,
                    TeacherFullName = fullName,
                    ProfileImageUrl = m.ProfileImageUrl, // 👈 ici
                    CreatedAt = m.CreatedAt,
                    UpdatedAt = m.UpdatedAt,
                    Evaluated = m.Evaluated
                });
            }

            return results;
        }

        
        public async Task<IEnumerable<ModuleDto>> GetFilteredModulesAsync(string filter)
        {
            IQueryable<Module> query = _context.Modules.AsQueryable();

            switch (filter.ToLower())
            {
                case "all":
                    query = query.AsNoTracking();
                    break;

                case "recent":
                    query = query.OrderByDescending(m => m.CreatedAt).Take(8);
                    break;

                case "evaluated":
                    query = query.Where(m => m.Evaluated == true);
                    break;

                case "notevaluated":
                    query = query.Where(m => m.Evaluated == false);
                    break;

                default:
                    return Enumerable.Empty<ModuleDto>();
            }

            var modules = await query
                .Include(m => m.Filiere)
                .ToListAsync();

            var results = new List<ModuleDto>();

            foreach (var m in modules)
            {
                var fullName = await _authHttp.GetTeacherFullNameAsync(m.TeacherId);

                results.Add(new ModuleDto
                {
                    ModuleId = m.ModuleId,
                    ModuleName = m.ModuleName,
                    ModuleDescription = m.ModuleDescription,
                    ModuleDuration = m.ModuleDuration,
                    FiliereName = m.Filiere.FiliereName,
                    ProfileImageUrl = m.ProfileImageUrl, // 👈 ici
                    TeacherId = m.TeacherId,
                    TeacherFullName = fullName,
                    CreatedAt = m.CreatedAt,
                    UpdatedAt = m.UpdatedAt,
                    Evaluated = m.Evaluated
                });
            }

            return results;
        }
        
        public async Task<bool> UpdateModuleImageUrlAsync(int moduleId, string imageUrl)
        {
            var module = await _context.Modules.FindAsync(moduleId);
            if (module == null) return false;

            module.ProfileImageUrl = imageUrl;
            module.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return true;
        }
        
        public async Task<IEnumerable<UnassignedModuleDto>> GetUnassignedModulesAsync()
        {
            var modules = await _context.Modules
                .Where(m => m.TeacherId == 0)
                .Select(m => new UnassignedModuleDto
                {
                    ModuleId = m.ModuleId,
                    ModuleName = m.ModuleName
                })
                .ToListAsync();

            return modules;
        }

        public async Task<bool> AssignModulesToTeacherAsync(int teacherId, List<int> moduleIds)
        {
            var modules = await _context.Modules
                .Where(m => moduleIds.Contains(m.ModuleId))
                .ToListAsync();

            if (!modules.Any())
                return false;

            foreach (var module in modules)
            {
                module.TeacherId = teacherId;
                module.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<string?> GetTeacherFullNameByModuleIdAsync(int moduleId)
        {
            var module = await _context.Modules.FindAsync(moduleId);
            if (module == null) return null;

            return await _authHttp.GetTeacherFullNameAsync(module.TeacherId);
        }



        public async Task<IEnumerable<ModuleDto>> GetModulesByTeacherAsync(int teacherId)
        {
            // Récupère les modules dont TeacherId correspond
            var modules = await _context.Modules
                .Where(m => m.TeacherId == teacherId)
                .ToListAsync();
            var results = new List<ModuleDto>(); 
            foreach (var m in modules)
            {
                var fullName = await _authHttp.GetTeacherFullNameAsync(m.TeacherId);

                results.Add(new ModuleDto
                {
                    ModuleId = m.ModuleId,
                    ModuleName = m.ModuleName,
                    ModuleDescription = m.ModuleDescription,
                    ModuleDuration = m.ModuleDuration,
                    FiliereName = m.Filiere.FiliereName,
                    TeacherId = m.TeacherId,
                    TeacherFullName = fullName,
                    CreatedAt = m.CreatedAt,
                    UpdatedAt = m.UpdatedAt,
                    Evaluated = m.Evaluated
                });
        }
    return results;}

    
    }
}

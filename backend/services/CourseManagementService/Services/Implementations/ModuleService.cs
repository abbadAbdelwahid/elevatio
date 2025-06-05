// /Services/Implementations/ModuleService.cs
using CourseManagementService.DTOs;
using CourseManagementService.Models;
using CourseManagementService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementService.Services.Implementations
{
    public class ModuleService : IModuleService
    {
        private readonly ApplicationDbContext _context;

        public ModuleService(ApplicationDbContext context)
        {
            _context = context;
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
                TeacherId = module.TeacherId,
                CreatedAt = module.CreatedAt,
                UpdatedAt = module.UpdatedAt
            };
        }

        public async Task<ModuleDto?> GetModuleByIdAsync(int id)
        {
            var module = await _context.Modules
                .Include(m => m.Filiere) // Inclure les données de la filière associée
                .FirstOrDefaultAsync(m => m.ModuleId == id);

            if (module == null)
                return null;

            return new ModuleDto
            {
                ModuleId = module.ModuleId,
                ModuleName = module.ModuleName,
                ModuleDescription = module.ModuleDescription,
                ModuleDuration = module.ModuleDuration,
                FiliereName = module.Filiere.FiliereName, // Récupérer le nom de la filière
                TeacherId = module.TeacherId,
                CreatedAt = module.CreatedAt,
                UpdatedAt = module.UpdatedAt
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

            return modules.Select(m => new ModuleDto
            {
                ModuleId = m.ModuleId,
                ModuleName = m.ModuleName,
                ModuleDescription = m.ModuleDescription,
                ModuleDuration = m.ModuleDuration,
                FiliereName = m.Filiere.FiliereName,
                TeacherId = m.TeacherId,
                CreatedAt = m.CreatedAt,
                UpdatedAt = m.UpdatedAt
            });
        }
        
        public async Task<IEnumerable<ModuleDto>> GetFilteredModulesAsync(string filter)
        {
            IQueryable<Module> query = _context.Modules.AsQueryable();

            switch (filter.ToLower())
            {
                case "all":
                    // Retourne tous les modules
                    query = query.AsNoTracking();
                    break;
            
                case "recent":
                    // Retourne les 8 derniers modules créés
                    query = query.OrderByDescending(m => m.CreatedAt).Take(8);
                    break;

                case "evaluated":
                    // Retourne tous les modules avec Evaluated = true
                    query = query.Where(m => m.Evaluated == true);
                    break;

                case "notevaluated":
                    // Retourne tous les modules avec Evaluated = false
                    query = query.Where(m => m.Evaluated == false);
                    break;

                default:
                    return Enumerable.Empty<ModuleDto>(); // Si un filtre invalide est passé
            }

            var modules = await query
                .Include(m => m.Filiere) // Inclure les données de la filière associée
                .ToListAsync();

            return modules.Select(m => new ModuleDto
            {
                ModuleId = m.ModuleId,
                ModuleName = m.ModuleName,
                ModuleDescription = m.ModuleDescription,
                ModuleDuration = m.ModuleDuration,
                FiliereName = m.Filiere.FiliereName,
                TeacherId = m.TeacherId,
                CreatedAt = m.CreatedAt,
                UpdatedAt = m.UpdatedAt,
                Evaluated = m.Evaluated // Inclure la nouvelle propriété Evaluated
            });
        }

    
    }
}

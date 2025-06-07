using CourseManagementService.DTOs;
using CourseManagementService.Models;                                   // pour Filiere :contentReference[oaicite:0]{index=0}:contentReference[oaicite:1]{index=1}
using CourseManagementService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseManagementService.Services.Implementations
{
    public class FiliereService : IFiliereService
    {
        private readonly ApplicationDbContext _context;

        public FiliereService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<FiliereDto> CreateFiliereAsync(CreateFiliereDto dto)
        {
            // Vérifie si une filière avec le même nom (insensible à la casse) existe déjà
            var exists = await _context.Filieres
                .AnyAsync(f => f.FiliereName.ToLower() == dto.FiliereName.ToLower());

            if (exists)
                throw new InvalidOperationException("Une filière avec ce nom existe déjà.");

            
            var entity = new Filiere
            {
                FiliereName = dto.FiliereName,
                Description  = dto.Description,
                CreatedAt    = DateTime.UtcNow,
                UpdatedAt    = DateTime.UtcNow
            };

            _context.Filieres.Add(entity);
            await _context.SaveChangesAsync();

            return new FiliereDto
            {
                FiliereId    = entity.FiliereId,
                FiliereName  = entity.FiliereName,
                Description  = entity.Description,
                CreatedAt    = entity.CreatedAt,
                UpdatedAt    = entity.UpdatedAt
            };
        }

        public async Task<IEnumerable<FiliereDto>> GetAllFilieresAsync()
        {
            var list = await _context.Filieres
                .AsNoTracking()
                .OrderBy(f => f.FiliereName)
                .ToListAsync();

            return list.Select(f => new FiliereDto
            {
                FiliereId    = f.FiliereId,
                FiliereName  = f.FiliereName,
                Description  = f.Description,
                CreatedAt    = f.CreatedAt,
                UpdatedAt    = f.UpdatedAt
            });
        }
        
        public async Task<FiliereDto?> UpdateFiliereAsync(int id, UpdateFiliereDto dto)
        {
            var entity = await _context.Filieres.FindAsync(id);
            if (entity == null) 
                return null;
            
            // Vérifie qu'aucune autre filière (autre que celle en cours) n’a ce nom
            var exists = await _context.Filieres
                .AnyAsync(f =>
                    f.FiliereId != id &&
                    f.FiliereName.ToLower() == dto.FiliereName.ToLower()
                );

            if (exists)
                throw new InvalidOperationException("Une filière avec ce nom existe déjà.");



            entity.FiliereName = dto.FiliereName;
            entity.Description = dto.Description;
            entity.UpdatedAt   = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new FiliereDto
            {
                FiliereId    = entity.FiliereId,
                FiliereName  = entity.FiliereName,
                Description  = entity.Description,
                CreatedAt    = entity.CreatedAt,
                UpdatedAt    = entity.UpdatedAt
            };
        }
        
        public async Task<bool> DeleteFiliereAsync(int id)
        {
            var entity = await _context.Filieres.FindAsync(id);
            if (entity == null) 
                return false;

            _context.Filieres.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<FiliereDto?> GetFiliereByIdAsync(int id)
        {
            var filiere = await _context.Filieres.FindAsync(id);
            if (filiere == null)
                return null;

            return new FiliereDto
            {
                FiliereId   = filiere.FiliereId,
                FiliereName = filiere.FiliereName,
                Description = filiere.Description,
                CreatedAt   = filiere.CreatedAt,
                UpdatedAt   = filiere.UpdatedAt
            };
        }
        
        public async Task<IEnumerable<FiliereMiniDto>> GetFiliereIdsAndNamesAsync()
        {
            return await _context.Filieres
                .AsNoTracking()
                .Select(f => new FiliereMiniDto
                {
                    FiliereId = f.FiliereId,
                    FiliereName = f.FiliereName
                })
                .ToListAsync();
        }


        
    }
}

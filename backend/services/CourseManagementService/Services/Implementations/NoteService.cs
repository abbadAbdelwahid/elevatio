// /Services/Implementations/NoteService.cs
using CourseManagementService.DTOs;
using CourseManagementService.Models;
using CourseManagementService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementService.Services.Implementations
{
    public class NoteService : INoteService
    {
        private readonly ApplicationDbContext _context;

        public NoteService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<NoteDto> CreateNoteAsync(CreateNoteDto dto)
        {
            // 1) Vérifier que le module existe
            var module = await _context.Modules
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ModuleId == dto.ModuleId);

            if (module == null)
                throw new InvalidOperationException("Module introuvable.");

            // 2) Créer l’entité Note
            var note = new Note
            {
                ModuleId  = dto.ModuleId,
                StudentId = dto.StudentId,
                Grade     = dto.Grade,
                Comment   = dto.Comment?.Trim(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Notes.Add(note);
            await _context.SaveChangesAsync();

            // 3) Retourner le DTO
            return new NoteDto
            {
                NoteId     = note.NoteId,
                ModuleId   = note.ModuleId,
                ModuleName = module.ModuleName,
                StudentId  = note.StudentId,
                Grade      = note.Grade,
                Comment    = note.Comment,
                CreatedAt  = note.CreatedAt,
                UpdatedAt  = note.UpdatedAt
            };
        }
        
        // --------- Mise à jour ---------
    public async Task<NoteDto?> UpdateNoteAsync(int noteId, UpdateNoteDto dto)
    {
        var note = await _context.Notes
                                 .Include(n => n.Module)
                                 .FirstOrDefaultAsync(n => n.NoteId == noteId);

        if (note == null) return null;

        note.Grade   = dto.Grade;
        note.Comment = dto.Comment?.Trim();
        note.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return ToDto(note, note.Module.ModuleName);
    }

    // --------- Suppression ---------
    public async Task<bool> DeleteNoteAsync(int noteId)
    {
        var note = await _context.Notes.FindAsync(noteId);
        if (note == null) return false;

        _context.Notes.Remove(note);
        await _context.SaveChangesAsync();
        return true;
    }

    // --------- Liste globale ---------
    public async Task<IEnumerable<NoteDto>> GetAllNotesAsync()
    {
        var list = await _context.Notes
                                 .Include(n => n.Module)
                                 .AsNoTracking()
                                 .ToListAsync();

        return list.Select(n => ToDto(n, n.Module.ModuleName));
    }

    // --------- Liste par module ---------
    public async Task<IEnumerable<NoteDto>> GetNotesByModuleAsync(int moduleId)
    {
        var list = await _context.Notes
                                 .Include(n => n.Module)
                                 .Where(n => n.ModuleId == moduleId)
                                 .AsNoTracking()
                                 .ToListAsync();

        return list.Select(n => ToDto(n, n.Module.ModuleName));
    }

    // --------- Helper privé ---------
    private static NoteDto ToDto(Note note, string moduleName) => new()
    {
        NoteId     = note.NoteId,
        ModuleId   = note.ModuleId,
        ModuleName = moduleName,
        StudentId  = note.StudentId,
        Grade      = note.Grade,
        Comment    = note.Comment,
        CreatedAt  = note.CreatedAt,
        UpdatedAt  = note.UpdatedAt
    };
    }
}
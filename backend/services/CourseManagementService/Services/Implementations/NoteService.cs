// /Services/Implementations/NoteService.cs
using CourseManagementService.DTOs;
using CourseManagementService.ExternalServices;
using CourseManagementService.Models;
using CourseManagementService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementService.Services.Implementations
{
    public class NoteService : INoteService
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthHttpClientService _authHttp;

        public NoteService(ApplicationDbContext context,AuthHttpClientService authHttp)
        {
            _context = context;
            _authHttp = authHttp;

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
        var notes = await _context.Notes
            .Include(n => n.Module)
            .AsNoTracking()
            .ToListAsync();

        var results = new List<NoteDto>();

        foreach (var note in notes)
        {
            var studentFullName = await _authHttp.GetStudentFullNameAsync(note.StudentId);

            results.Add(new NoteDto
            {
                NoteId     = note.NoteId,
                ModuleId   = note.ModuleId,
                ModuleName = note.Module.ModuleName,
                StudentId  = note.StudentId,
                StudentFullName = studentFullName,
                Grade      = note.Grade,
                Comment    = note.Comment,
                CreatedAt  = note.CreatedAt,
                UpdatedAt  = note.UpdatedAt
            });
        }

        return results;
    }


    // --------- Liste par module ---------
    public async Task<IEnumerable<NoteDto>> GetNotesByModuleAsync(int moduleId)
    {
        var notes = await _context.Notes
            .Include(n => n.Module)
            .Where(n => n.ModuleId == moduleId)
            .AsNoTracking()
            .ToListAsync();

        var results = new List<NoteDto>();

        foreach (var note in notes)
        {
            var studentFullName = await _authHttp.GetStudentFullNameAsync(note.StudentId);

            results.Add(new NoteDto
            {
                NoteId = note.NoteId,
                ModuleId = note.ModuleId,
                ModuleName = note.Module.ModuleName,
                StudentId = note.StudentId,
                StudentFullName = studentFullName,
                Grade = note.Grade,
                Comment = note.Comment,
                CreatedAt = note.CreatedAt,
                UpdatedAt = note.UpdatedAt
            });
        }

        return results;
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
    
    
    public async Task<IEnumerable<NoteDto>> GetNotesByStudentAsync(int studentId)
    {
        var notes = await _context.Notes
            .Include(n => n.Module)
            .Where(n => n.StudentId == studentId)
            .AsNoTracking()
            .ToListAsync();

        var studentFullName = await _authHttp.GetStudentFullNameAsync(studentId);

        return notes.Select(n => new NoteDto
        {
            NoteId = n.NoteId,
            ModuleId = n.ModuleId,
            ModuleName = n.Module.ModuleName,
            StudentId = n.StudentId,
            StudentFullName = studentFullName,
            Grade = n.Grade,
            Comment = n.Comment,
            CreatedAt = n.CreatedAt,
            UpdatedAt = n.UpdatedAt
        });
    }

    
    public async Task<NoteDto?> GetNoteWithStudentNameAsync(int id)
    {
        var note = await _context.Notes
            .Include(n => n.Module)
            .FirstOrDefaultAsync(n => n.NoteId == id);

        if (note == null) return null;

        var studentFullName = await _authHttp.GetStudentFullNameAsync(note.StudentId);

        return new NoteDto
        {
            NoteId     = note.NoteId,
            ModuleId   = note.ModuleId,
            ModuleName = note.Module.ModuleName,
            StudentId  = note.StudentId,
            StudentFullName = studentFullName,
            Grade      = note.Grade,
            Comment    = note.Comment,
            CreatedAt  = note.CreatedAt,
            UpdatedAt  = note.UpdatedAt
        };
    }


    }
}
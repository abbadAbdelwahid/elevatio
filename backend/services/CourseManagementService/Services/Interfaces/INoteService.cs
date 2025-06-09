// /Services/Interfaces/INoteService.cs
using CourseManagementService.DTOs;

namespace CourseManagementService.Services.Interfaces
{
    public interface INoteService
    {
        Task<NoteDto>                CreateNoteAsync(CreateNoteDto dto);
        Task<NoteDto?>               UpdateNoteAsync(int noteId, UpdateNoteDto dto);
        Task<bool>                   DeleteNoteAsync(int noteId);
        Task<IEnumerable<NoteDto>>   GetAllNotesAsync();
        Task<IEnumerable<NoteDto>>   GetNotesByModuleAsync(int moduleId);
        Task<IEnumerable<NoteDto>> GetNotesByStudentAsync(int studentId);

        public Task<NoteDto?> GetNoteWithStudentNameAsync(int id);
        Task<double?> GetStudentAverageAsync(int studentId);
        
        Task<IEnumerable<StudentNoteBriefDto>> GetStudentNotesByModuleNameAsync(string moduleName);




    }
}
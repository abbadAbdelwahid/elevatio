// /Controllers/NoteController.cs
using CourseManagementService.DTOs;
using CourseManagementService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagementService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NoteController : ControllerBase
    {
        private readonly INoteService _service;

        public NoteController(INoteService service)
        {
            _service = service;
        }

        // POST api/note
        [HttpPost]
        public async Task<ActionResult<NoteDto>> Create([FromBody] CreateNoteDto dto)
        {
            var result = await _service.CreateNoteAsync(dto);
            return CreatedAtAction(nameof(Create), new { id = result.NoteId }, result);
        }
        
        // PUT api/note/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<NoteDto>> Update(int id, [FromBody] UpdateNoteDto dto)
        {
            var result = await _service.UpdateNoteAsync(id, dto);
            if (result == null) return NotFound();
            return Ok(result);
        }

        // DELETE api/note/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteNoteAsync(id);
            return ok ? NoContent() : NotFound();
        }

        // GET api/note
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NoteDto>>> GetAll()
            => Ok(await _service.GetAllNotesAsync());

        // GET api/note/module/{moduleId}
        [HttpGet("module/{moduleId}")]
        public async Task<ActionResult<IEnumerable<NoteDto>>> GetByModule(int moduleId)
            => Ok(await _service.GetNotesByModuleAsync(moduleId));

        
        // GET api/note/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<NoteDto>> GetById(int id)
        {
            var note = await _service.GetNoteWithStudentNameAsync(id);
            return note == null ? NotFound() : Ok(note);
        }

        
        
        // GET api/note/student/{studentId}
        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<IEnumerable<NoteDto>>> GetByStudent(int studentId)
        {
            var notes = await _service.GetNotesByStudentAsync(studentId);
            return Ok(notes);
        }
        
        // GET api/note/student/{studentId}/average
        [HttpGet("student/{studentId}/average")]
        public async Task<ActionResult<double>> GetStudentAverage(int studentId)
        {
            var avg = await _service.GetStudentAverageAsync(studentId);
            if (avg == null)
                return NotFound("Aucune note trouvée pour cet étudiant.");

            return Ok(new { studentId, average = avg });
        }


    }
}
using CourseManagementService.DTOs;
using CourseManagementService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseManagementService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FiliereController : ControllerBase
    {
        private readonly IFiliereService _service;

        public FiliereController(IFiliereService service)
        {
            _service = service;
        }

        // POST api/filiere
        [HttpPost]
        public async Task<ActionResult<FiliereDto>> Create([FromBody] CreateFiliereDto dto)
        {
            var result = await _service.CreateFiliereAsync(dto);
            // Retourne 201 Created + URI vers GET
            return CreatedAtAction(nameof(GetAll), new { id = result.FiliereId }, result);
        }

        // GET api/filiere
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FiliereDto>>> GetAll()
        {
            var list = await _service.GetAllFilieresAsync();
            return Ok(list);
        }
        
        // PUT api/filiere/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<FiliereDto>> Update(int id, [FromBody] UpdateFiliereDto dto)
        {
            var updated = await _service.UpdateFiliereAsync(id, dto);
            if (updated == null)
                return NotFound();    // 404 si la filière n'existe pas
            return Ok(updated);
        }

        // DELETE api/filiere/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteFiliereAsync(id);
            if (!success)
                return NotFound();
            return NoContent();       // 204 No Content si suppression réussie
        }
        
        // GET api/filiere/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<FiliereDto>> GetById(int id)
        {
            var filiere = await _service.GetFiliereByIdAsync(id);
            if (filiere == null)
                return NotFound(); // 404

            return Ok(filiere); // 200
        }
        
        // GET api/filiere/mini
        [HttpGet("mini")]
        public async Task<ActionResult<IEnumerable<FiliereMiniDto>>> GetIdsAndNames()
        {
            var list = await _service.GetFiliereIdsAndNamesAsync();
            return Ok(list);
        }


        
    }
}
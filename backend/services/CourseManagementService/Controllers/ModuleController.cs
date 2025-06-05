// /Controllers/ModuleController.cs
using CourseManagementService.DTOs;
using CourseManagementService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CourseManagementService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ModuleController : ControllerBase
    {
        private readonly IModuleService _service;

        public ModuleController(IModuleService service)
        {
            _service = service;
        }

        // POST api/module
        [HttpPost]
        public async Task<ActionResult<ModuleDto>> Create([FromBody] CreateModuleDto dto)
        {
            var result = await _service.CreateModuleAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.ModuleId }, result);
        }

        // GET api/module/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ModuleDto>> GetById(int id)
        {
            var module = await _service.GetModuleByIdAsync(id);
            if (module == null)
                return NotFound(); // 404

            return Ok(module); // 200
        }
        
        
        // GET api/module?filter={filter}
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModuleDto>>> GetAll([FromQuery] string filter)
        {
            var modules = await _service.GetFilteredModulesAsync(filter);
            return Ok(modules); // 200
        }


        // PUT api/module/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<ModuleDto>> Update(int id, [FromBody] UpdateModuleDto dto)
        {
            var updated = await _service.UpdateModuleAsync(id, dto);
            if (updated == null)
                return NotFound();    // 404 si le module n'existe pas
            return Ok(updated);
        }

        // DELETE api/module/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteModuleAsync(id);
            if (!success)
                return NotFound();
            return NoContent();       // 204 No Content si suppression réussie
        }
        
        // GET api/module/filiere/{filiereName}
        [HttpGet("filiere/{filiereName}")]
        public async Task<ActionResult<IEnumerable<ModuleDto>>> GetModulesByFiliere(string filiereName)
        {
            var result = await _service.GetModulesByFiliereNameAsync(filiereName);
            return Ok(result);
        }


    }
}

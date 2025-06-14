using CourseManagementService.DTOs;
using CourseManagementService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CourseManagementService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _service;

        public ScheduleController(IScheduleService service)
        {
            _service = service;
        }

        // GET api/schedule?group={group}&year={year}&week={week}
        [HttpGet]
        public async Task<ActionResult<ScheduleDto>> GetSchedule(string group, string year, string week)
        {
            var schedule = await _service.GetScheduleByGroupAsync(group, year, week);
            if (schedule == null)
                return NotFound();

            return Ok(schedule);
        }

        // POST api/schedule
        [HttpPost]
        public async Task<ActionResult<ScheduleDto>> CreateSchedule([FromBody] ScheduleDto dto)
        {
            var schedule = await _service.CreateScheduleAsync(dto);
            return CreatedAtAction(nameof(GetSchedule), new { group = schedule.Group, year = schedule.Year, week = schedule.Week }, schedule);
        }
        
        // GET api/schedule/courses
        [HttpGet("courses")]
        public async Task<ActionResult<IEnumerable<CourseScheduleBriefDto>>> GetAllCourses()
        {
            var result = await _service.GetAllCourseSchedulesAsync();
            return Ok(result);
        }
        
        // DELETE api/schedule/courses/{id}
        [HttpDelete("courses/{id}")]
        public async Task<IActionResult> DeleteCourseSchedule(int id)
        {
            var deleted = await _service.DeleteCourseScheduleAsync(id);
            if (!deleted)
                return NotFound("CourseSchedule introuvable.");

            return NoContent(); // 204
        }
        
        // PUT api/schedule/courses/{id}
        [HttpPut("courses/{id}")]
        public async Task<IActionResult> UpdateCourseSchedule(int id, [FromBody] UpdateCourseScheduleDto dto)
        {
            var updated = await _service.UpdateCourseScheduleAsync(id, dto);
            if (!updated)
                return NotFound("CourseSchedule introuvable.");

            return Ok("CourseSchedule mis à jour avec succès.");
        }



    }
}
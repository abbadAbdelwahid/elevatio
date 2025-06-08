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
    }
}
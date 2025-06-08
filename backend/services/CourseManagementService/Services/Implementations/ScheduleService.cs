using CourseManagementService.DTOs;
using CourseManagementService.Models;
using CourseManagementService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseManagementService.Services.Implementations
{
    public class ScheduleService : IScheduleService
    {
        private readonly ApplicationDbContext _context;

        public ScheduleService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Méthode pour récupérer un emploi du temps par groupe, année et semaine
        public async Task<ScheduleDto> GetScheduleByGroupAsync(string group, string year, string week)
        {
            var schedule = await _context.Schedules
                .Include(s => s.Courses)
                .ThenInclude(c => c.Module) // Inclure les modules associés
                .FirstOrDefaultAsync(s => s.Group == group && s.Year == year && s.Week == week);

            if (schedule == null)
                return null;

            return new ScheduleDto
            {
                Group = schedule.Group,
                Year = schedule.Year,
                Week = schedule.Week,
                Courses = schedule.Courses.Select(c => new CourseScheduleDto
                {
                    Day = c.Day,
                    Start = c.Start,
                    End = c.End,
                    ModuleId = c.ModuleId,
                    Location = c.Location
                }).ToList()
            };
        }

        // Méthode pour créer un emploi du temps
        public async Task<ScheduleDto> CreateScheduleAsync(ScheduleDto dto)
        {
            var schedule = new Schedule
            {
                Group = dto.Group,
                Year = dto.Year,
                Week = dto.Week,
                Courses = dto.Courses.Select(c => new CourseSchedule
                {
                    Day = c.Day,
                    Start = c.Start,
                    End = c.End,
                    ModuleId = c.ModuleId,
                    Location = c.Location
                }).ToList()
            };

            _context.Schedules.Add(schedule);
            await _context.SaveChangesAsync();

            return new ScheduleDto
            {
                Group = schedule.Group,
                Year = schedule.Year,
                Week = schedule.Week,
                Courses = schedule.Courses.Select(c => new CourseScheduleDto
                {
                    Day = c.Day,
                    Start = c.Start,
                    End = c.End,
                    ModuleId = c.ModuleId,
                    Location = c.Location
                }).ToList()
            };
        }
    }
}

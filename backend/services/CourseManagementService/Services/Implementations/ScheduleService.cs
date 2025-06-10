using CourseManagementService.DTOs;
using CourseManagementService.Models;
using CourseManagementService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseManagementService.ExternalServices;

namespace CourseManagementService.Services.Implementations
{
    public class ScheduleService : IScheduleService
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthHttpClientService _authHttp;


        public ScheduleService(ApplicationDbContext context, AuthHttpClientService authHttp)
        {
            _context = context;
            _authHttp = authHttp;

        }

        // Méthode pour récupérer un emploi du temps par groupe, année et semaine
        public async Task<ScheduleDto> GetScheduleByGroupAsync(string group, string year, string week)
        {
            var schedule = await _context.Schedules
                .Include(s => s.Courses)
                .ThenInclude(c => c.Module)
                .FirstOrDefaultAsync(s => s.Group == group && s.Year == year && s.Week == week);

            if (schedule == null)
                return null;

            var result = new ScheduleDto
            {
                Group = schedule.Group,
                Year = schedule.Year,
                Week = schedule.Week,
                Courses = new List<CourseScheduleDto>()
            };

            foreach (var course in schedule.Courses)
            {
                var teacherFullName = await _authHttp.GetTeacherFullNameAsync(course.Module.TeacherId);

                result.Courses.Add(new CourseScheduleDto
                {
                    CourseScheduleId = course.CourseScheduleId,
                    Day = course.Day,
                    Start = course.Start,
                    End = course.End,
                    ModuleId = course.ModuleId,
                    Location = course.Location,
                    TeacherFullName = teacherFullName
                });
            }

            return result;
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
        
        public async Task<IEnumerable<CourseScheduleBriefDto>> GetAllCourseSchedulesAsync()
        {
            var courses = await _context.CourseSchedules
                .Include(cs => cs.Module)
                .ToListAsync();

            var result = new List<CourseScheduleBriefDto>();

            foreach (var cs in courses)
            {
                var teacherFullName = await _authHttp.GetTeacherFullNameAsync(cs.Module.TeacherId);

                result.Add(new CourseScheduleBriefDto
                {
                    CourseScheduleId = cs.CourseScheduleId,
                    Day = cs.Day,
                    Start = cs.Start,
                    End = cs.End,
                    ModuleId = cs.ModuleId,
                    Location = cs.Location,
                    ScheduleId = cs.ScheduleId,
                    TeacherFullName = teacherFullName
                });
            }

            return result;
        }

        
        public async Task<bool> DeleteCourseScheduleAsync(int courseScheduleId)
        {
            var course = await _context.CourseSchedules.FindAsync(courseScheduleId);
            if (course == null) return false;

            _context.CourseSchedules.Remove(course);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateCourseScheduleAsync(int courseScheduleId, UpdateCourseScheduleDto dto)
        {
            var course = await _context.CourseSchedules.FindAsync(courseScheduleId);
            if (course == null)
                return false;

            course.Day = dto.Day;
            course.Start = dto.Start;
            course.End = dto.End;
            course.ModuleId = dto.ModuleId;
            course.Location = dto.Location;

            await _context.SaveChangesAsync();
            return true;
        }


    }
}

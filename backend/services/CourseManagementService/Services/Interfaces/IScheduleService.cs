using CourseManagementService.DTOs;
using System.Threading.Tasks;

namespace CourseManagementService.Services.Interfaces
{
    public interface IScheduleService
    {
        // Méthode pour récupérer un emploi du temps par groupe, année et semaine
        Task<ScheduleDto> GetScheduleByGroupAsync(string group, string year, string week);

        // Méthode pour créer un emploi du temps
        Task<ScheduleDto> CreateScheduleAsync(ScheduleDto dto);
    }
}
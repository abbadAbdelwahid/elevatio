namespace AnalyticsService.Services.Interfaces;
using AnalyticsService.Models;
public interface IStatistiqueService<T>
{
    Task<T> CalculateStats(int Id);
}
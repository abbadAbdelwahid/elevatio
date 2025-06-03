namespace AnalyticsService.Services.Interfaces;
using AnalyticsService.Models;
public interface IStatistiqueMFService<T>
{
    Task<T> CalculateStats(int Id);
}
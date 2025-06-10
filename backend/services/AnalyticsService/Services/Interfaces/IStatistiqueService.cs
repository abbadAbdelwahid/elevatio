namespace AnalyticsService.Services.Interfaces;
using AnalyticsService.Models;
public interface IStatistiqueService<T>
{
     Task<T> CreateAsync(int Id,String FiliereName); 
     Task<T> GetByPropertyAsync(int Id);
     Task<T> CalculateStandardStats(int Id);
     Task<T> CalculateMarksStats(String FiliereName );
     Task<double> CalculateAndStoreAverageRatingAsync(int moduleId);   
     
}
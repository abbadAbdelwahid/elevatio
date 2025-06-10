namespace AnalyticsService.Services.Interfaces;

public interface IStatistiqueModuleService <T> 
{
    Task<T> CreateAsync(int Id); 
    Task<T> GetByPropertyAsync(int Id);
    Task<T> CalculateStandardStats(int Id);
    Task<T> CalculateMarksStats(int Id );
    Task<double> CalculateAndStoreAverageRatingAsync(int moduleId);   
}
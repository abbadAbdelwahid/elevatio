namespace AnalyticsService.Services.Interfaces;

public interface IStatistiqueUserService<T>
{
    Task<T> CreateAsync(int Id);
    Task<T> CalculateStats(int Id);  
    Task<T> GetByUserIdAsync(int Id); 
    
}
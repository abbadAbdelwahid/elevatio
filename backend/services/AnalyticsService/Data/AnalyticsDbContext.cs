using Microsoft.EntityFrameworkCore;
using AnalyticsService.Models;


namespace AnalyticsService.Data;

public class AnalyticsDbContext : DbContext 
{
    public AnalyticsDbContext(DbContextOptions<AnalyticsDbContext> options)
        : base(options)
    {
    }

    public DbSet<StatistiqueFiliere>    StatistiquesFilieres    { get; set; }
    public DbSet<StatistiqueModule>     StatistiquesModules     { get; set; }
    public DbSet<StatistiqueEnseignant> StatistiquesEnseignants { get; set; } 
   

    // Rapports par questionnaire
    public DbSet<RapportFQ> RapportFQs { get; set; }
    public DbSet<RapportMQ> RapportMQs { get; set; }

    // Rapport enseignant
    
}

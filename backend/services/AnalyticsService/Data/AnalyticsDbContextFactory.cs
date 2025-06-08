using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AnalyticsService.Data;

public class AnalyticsDbContextFactory  : IDesignTimeDbContextFactory<AnalyticsDbContext>
{
    public AnalyticsDbContext CreateDbContext(string[] args)
    {
        // 1. Construire la configuration (lit appsettings.json et ENV)
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables()
            .Build();

        // 2. Récupérer la chaîne de connexion
        var connString = config.GetConnectionString("AnalyticsDb");

        // 3. Construire les options EF Core
        var optionsBuilder = new DbContextOptionsBuilder<AnalyticsDbContext>();
        optionsBuilder.UseNpgsql(connString);

        // 4. Retourner une instance configurée
        return new AnalyticsDbContext(optionsBuilder.Options);
    } 
}
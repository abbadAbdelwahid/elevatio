using Microsoft.EntityFrameworkCore;

namespace AnalyticsService.Services.Implementations;
using AnalyticsService.Services.Interfaces; 
using AnalyticsService.ExternalClients.ClientInterfaces; 
using AnalyticsService.Models;
using AnalyticsService.ExternalClients.DTO; 
using AnalyticsService.Data;

public class StatistiqueEnsService : IStatistiqueUserService<StatistiqueEnseignant>
{

    private readonly AnalyticsDbContext _db;
    private readonly INoteClient _noteClient;
    private readonly IModuleClient _modClient;


    public StatistiqueEnsService(AnalyticsDbContext db, INoteClient noteClient)
    {
        _db = db;
        _noteClient = noteClient;
    }
    public async Task<StatistiqueEnseignant> CreateAsync(int EnsId)
    {
        // On crée l’objet avec uniquement le ModuleId rempli
        var statistique = new StatistiqueEnseignant() {
            TeacherId = EnsId
            // Tous les autres champs restent à leur valeur par défaut (null ou 0)
        };

        _db.StatistiquesEnseignants.Add(statistique);
        await _db.SaveChangesAsync();

        return statistique;
    }
    public async Task<StatistiqueEnseignant> GetByUserIdAsync(int EnsId)
    {
        // On récupère la ligne stats pour le module
        var stats = await _db.StatistiquesEnseignants
            .FirstOrDefaultAsync(s => s.TeacherId==EnsId );

        if (stats == null)
        {
            // Vous pouvez choisir de retourner null ou de lancer une exception
            throw new KeyNotFoundException(
                $"Aucune statistique trouvée pour ens= {EnsId}");
        }

        return stats;
    }
   public async Task<StatistiqueEnseignant> CalculateStats(int teacherId)
    {
        // 1) Récupérer l’enregistrement existant depuis la base
        var statEns = await _db.StatistiquesEnseignants
            .FirstOrDefaultAsync(e => e.TeacherId == teacherId);

        if (statEns == null)
            throw new KeyNotFoundException($"Pas de statistiques trouvées pour l'enseignant {teacherId}.");

        // 2) Recalculer les stats de modules (via votre client ou service)
        var modules = await _modClient.GetModuleByTeacherAsync(teacherId);
        var moduleStats = await _db.StatistiquesModules
            .Where(sm => modules.Select(m => m.ModuleId).Contains(sm.ModuleId))
            .ToListAsync();

        var moyennes = moduleStats.Where(s => s.AverageRating.HasValue)
            .Select(s => s.AverageRating.Value)
            .ToList();

        var passRates = moduleStats.Where(s => s.PassRate.HasValue)
            .Select(s => s.PassRate.Value)
            .ToList();

        // 3) Appliquer les nouveaux calculs directement sur statEns
        statEns.AverageM = moyennes.Any() ? moyennes.Average() : 0;
        statEns.MedianNotes = CalculateMedian(moyennes);
        statEns.NoteMax = moyennes.Any() ? moyennes.Max() : 0;
        statEns.NoteMin = moyennes.Any() ? moyennes.Min() : 0;
        statEns.PassRate = passRates.Any() ? passRates.Average() : 0;
        statEns.CreatedAt = DateTime.UtcNow; // ou UpdatedAt

        // 4) Enregistrer les changements
        await _db.SaveChangesAsync();

        return statEns;
    }

    private double? CalculateMedian(List<double> values)
    {
        var sorted = values.OrderBy(v => v).ToList();
        int mid = sorted.Count / 2;
        return (sorted.Count % 2 == 0)
            ? (sorted[mid - 1] + sorted[mid]) / 2.0
            : sorted[mid];
    }
}
using AnalyticsService.Data;
using AnalyticsService.ExternalClients.ClientInterfaces;
using AnalyticsService.ExternalClients.DTO;
using AnalyticsService.Models;
using AnalyticsService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AnalyticsService.Services.Implementations;

public class StatistiqueEtdService : IStatistiqueUserService<StatistiqueEtudiant> 
{
    private readonly AnalyticsDbContext  _db; 
    private readonly INoteClient   _noteClient;

    public StatistiqueEtdService(AnalyticsDbContext db, INoteClient noteClient)
    {

        _db = db;
        _noteClient = noteClient;

    }
    public async Task<StatistiqueEtudiant> CreateAsync(int EtdId)
    {
        // On crée l’objet avec uniquement le ModuleId rempli
        var statistique = new StatistiqueEtudiant() {
            StudentId = EtdId
            // Tous les autres champs restent à leur valeur par défaut (null ou 0)
        };

        _db.StatistiquesEtudiants.Add(statistique);
        await _db.SaveChangesAsync();

        return statistique;
    } 
    public async Task<StatistiqueEtudiant> GetByUserIdAsync(int EtdId)
    {
        // On récupère la ligne stats pour le module
        var stats = await _db.StatistiquesEtudiants
            .FirstOrDefaultAsync(s => s.StudentId==EtdId );

        if (stats == null)
        {
            // Vous pouvez choisir de retourner null ou de lancer une exception
            throw new KeyNotFoundException(
                $"Aucune statistique trouvée pour etd= {EtdId}");
        }

        return stats;
    }
    public async Task<StatistiqueEtudiant> CalculateStats(int StudentId)
    {
        // 1) Récupérer les notes de l'étudiant
        var notes = (await _noteClient.GetByEtdAsync(StudentId))
            .ToList();
        
        if (!notes.Any())
            throw new InvalidOperationException($"Aucune note trouvée pour l'étudiant {StudentId}.");

        // 2) Construire l'objet résultat
        var stat = new StatistiqueEtudiant
        {
            StudentId   = StudentId,
            NoteMoyenne = notes.Average(n => n.Grade),
            NoteMax     = notes.Max(n => n.Grade),
            NoteMin     = notes.Min(n => n.Grade),
            PassRate    = CalculatePassRate(notes) ,
            Median = CalculateMedian(notes)  
            // Rap­portPdf = … (à générer plus tard)
        };

        return stat; 
        

    } 
    private double CalculateMedian(List<NoteDto> notes)
    { if (notes == null)
            throw new ArgumentNullException(nameof(notes));

        // Extraire les scores et les trier
        var sortedScores = notes
            .Select(n => (double)n.Grade)
            .OrderBy(score => score)
            .ToList();

        int count = sortedScores.Count;
        if (count == 0)
            return 0;

        int mid = count / 2;

        // Si le nombre d'éléments est pair, on fait la moyenne des deux du milieu
        if (count % 2 == 0)
        {
            return (sortedScores[mid - 1] + sortedScores[mid]) / 2.0;
        }

        // Sinon on retourne l'élément central
        return sortedScores[mid];
    }
    private double CalculatePassRate(IEnumerable<NoteDto> notes)
    {
        var total = notes.Count();
        if (total == 0) return 0;

        var passed = notes.Count(n => n.Grade >= 12.0);
        return (double) (passed / total)*100;
    } 
}
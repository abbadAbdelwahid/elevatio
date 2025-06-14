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
        // 1) Récupérer les notes de l’étudiant
        var notes = (await _noteClient.GetByEtdAsync(StudentId)).ToList();
    
        if (!notes.Any())
            throw new InvalidOperationException($"Aucune note trouvée pour l’étudiant {StudentId}.");

        // 2) Calcul des statistiques
        var moyenne = notes.Average(n => n.Grade);
        var noteMax = notes.Max(n => n.Grade);
        var noteMin = notes.Min(n => n.Grade);
        var passRate = CalculatePassRate(notes); // Implémentez la logique de passRate
        var mediane = CalculateMedian(notes);    // Implémentez la logique de médiane

        // 3) Chercher la statistique de l’étudiant existante dans la base de données
        var stat = await _db.StatistiquesEtudiants
            .FirstOrDefaultAsync(s => s.StudentId == StudentId);
    
        if (stat == null)
        {
            // Si aucune statistique n'existe, on crée une nouvelle instance
            stat = new StatistiqueEtudiant
            {
                StudentId = StudentId
            };
            _db.StatistiquesEtudiants.Add(stat); // Ajout dans le contexte
        }

        // 4) Mettre à jour les propriétés de la statistique
        stat.NoteMoyenne  = Math.Round(moyenne, 2);
        stat.NoteMax      = noteMax;
        stat.NoteMin      = noteMin;
        stat.PassRate     = Math.Round(passRate, 2);
        stat.Median       = mediane;
    
        // Vous pouvez aussi ajouter la génération du PDF ici si besoin
        // stat.RapportPdf = ... (à générer plus tard)

        // 5) Sauvegarder les changements dans la base de données
        await _db.SaveChangesAsync();

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
    {   var total = notes.Count();
        if (total == 0) return 0;

        var passed = notes.Count(n =>
        {
            Console.WriteLine($"Grade: {n.Grade}");  // Ajoutez cette ligne pour vérifier les valeurs des notes
            return n.Grade >= 12.0;
        });
        return (double)(passed) / total * 100;
    } 
}
using Microsoft.EntityFrameworkCore;

namespace AnalyticsService.Services.Implementations;
using AnalyticsService.Services.Interfaces; 
using AnalyticsService.ExternalClients.ClientInterfaces; 
using AnalyticsService.Models;
using AnalyticsService.ExternalClients.DTO; 
using AnalyticsService.Data;

public class StatistiqueFiliereService:IStatistiqueService<StatistiqueFiliere>
{
private readonly IEvaluationClient _evalClient; 
private readonly IModuleClient _moduleClient;
    private readonly IAnswerClient     _ansClient;
    private readonly IQuestionClient   _quesClient;
    private readonly IQuestionnaireClient _qClient;
    private readonly AnalyticsDbContext  _db;

    public StatistiqueFiliereService(
        IEvaluationClient       evalClient,
        IAnswerClient           ansClient,
        IQuestionClient         quesClient,
        IQuestionnaireClient    qClient,
        AnalyticsDbContext      db)
    {
        _evalClient = evalClient;
        _ansClient  = ansClient;
        _quesClient = quesClient;
        _qClient    = qClient;
        _db         = db;
    }
    public async Task<StatistiqueFiliere> CreateAsync(int FiliereId, String FiliereName)
    {
        // On crée l’objet avec uniquement le ModuleId rempli
        var statistique = new StatistiqueFiliere()
        {
            FiliereId = FiliereId , FiliereName = FiliereName
            // Tous les autres champs restent à leur valeur par défaut (null ou 0)
        };

        _db.StatistiquesFilieres.Add(statistique);
        await _db.SaveChangesAsync();

        return statistique;
    }
    public async Task<StatistiqueFiliere> GetByPropertyAsync(int FiliereId)
    {
        // On récupère la ligne stats pour le module
        var stats = await _db.StatistiquesFilieres
            .FirstOrDefaultAsync(s => s.FiliereId == FiliereId);

        if (stats == null)
        {
            // Vous pouvez choisir de retourner null ou de lancer une exception
            throw new KeyNotFoundException(
                $"Aucune statistique trouvée pour filiere = {FiliereId}");
        }

        return stats;
    }
    public async Task<StatistiqueFiliere> CalculateStandardStats(int filiereId)
    {
        // 1. Récupérer les questionnaires pour la filière
        var questionnaires = await _qClient.GetByFiliereAsync(filiereId);
        var allQuestions   = new List<QuestionDto>();
        foreach (var q in questionnaires)
        {
            var questions = await _quesClient.GetByQuestionnaireAsync(q.QuestionnaireId);
            allQuestions.AddRange(questions);
        }

        // 2. Récupérer toutes les réponses aux questions
        var allAnswers = new List<AnswerDto>(); 
         
        foreach (var question in allQuestions)
        {
            var answers = await _ansClient.GetByQuestionAsync(question.QuestionId);
            allAnswers.AddRange(answers);
        }

        // 3. Filtrer les questions sur lesquelles on calcule une stat
        var statQuestions = allQuestions
          .Where(q => !string.IsNullOrEmpty(q.StatName))
          .ToList();

        // 4. Préparer l’objet StatistiqueModule (on réutilise ce modèle)
        var stat = new StatistiqueFiliere()
        {
            FiliereId = filiereId,
            CreatedAt = DateTime.UtcNow
        };

        // 5. Pour chaque question « statistiquable », calculer et stocker
        foreach (var question in statQuestions)
        {
            var values = allAnswers
                .Where(a => a.QuestionId == question.QuestionId && a.RatingAnswer.HasValue)
                .Select(a => a.RatingAnswer.Value)
                .ToList();
            Console.WriteLine($"Total answers: {allAnswers.Count}"); 
            if (!values.Any()) continue;

            double? statValue = question.StatName switch
            {
                
                "SatisfactionRate"  => CalculateMedian(values),
                "StdDev"=> CalculateStdDev(values),
                "NpsScore"     => CalculateNps(values),
                _ => null 
            };

            switch (question.StatName)
            {
                
                case "Satisfacti{onRate":
                    stat.SatisfactionRate = statValue;
                    break;
                case "NpsScore":
                    stat.NpsScore = statValue;
                    break;
            }
        }

        // 6. Sauvegarde en base
        _db.StatistiquesFilieres.Add(stat);
        await _db.SaveChangesAsync();
        return stat;
    }

    public async Task<StatistiqueFiliere> CalculateMarksStats(String FiliereName)
    {
       // 1) Récupère la liste des modules de la filière
        var modules = (await _moduleClient.GetByFiliereAsync(FiliereName))
                          .ToList();
        if (!modules.Any())
            throw new KeyNotFoundException(
                $"Aucun module retourné pour la filière '{FiliereName}'.");

        // 2) Charge les stats existantes de ces modules
        var moduleIds = modules.Select(m => m.ModuleId).ToList();
        var statsMods = await _db.StatistiquesModules
            .Where(s => moduleIds.Contains(s.ModuleId))
            .ToListAsync();

        // 3) Agrège les indicateurs
        var moyennes    = statsMods
                            .Where(s => s.AverageNotes.HasValue)
                            .Select(s => s.AverageNotes!.Value);
        var ratings     = statsMods
                            .Where(s => s.AverageRating.HasValue)
                            .Select(s => s.AverageRating!.Value);
        var passRates   = statsMods
                            .Select(s => s.PassRate ?? 0);

        double? avgMoyenne = moyennes.Any()  ? moyennes.Average()  : (double?)null;
        double? avgRating  = ratings.Any()   ? ratings.Average()   : (double?)null;

        var maxPassStat  = statsMods.OrderByDescending(s => s.PassRate ?? 0)
                                    .First();
        var minPassStat  = statsMods.OrderBy(s            => s.PassRate ?? 0)
                                    .First();
        var MaxRatedModule = statsMods.OrderByDescending(s => s.AverageNotes ?? 0)
                                    .First();

        // 4) Récupère (ou crée) la ligne StatistiqueFiliere
        var filiereStats = await _db.StatistiquesFilieres
            .FirstOrDefaultAsync(f => f.FiliereName == FiliereName);
            

        // 5) Met à jour les champs
        filiereStats.AverageMoyenne  = avgMoyenne;
        filiereStats.AverageRating   = avgRating;
        filiereStats.ModuleMaxPass   = modules
                                         .First(m => m.ModuleId == maxPassStat.ModuleId)
                                         .ModuleName;
        filiereStats.ModuleMinPass   = modules
                                         .First(m => m.ModuleId == minPassStat.ModuleId)
                                         .ModuleName;
        filiereStats.MaxModuleRated         = modules
                                         .First(m => m.ModuleId == MaxRatedModule.ModuleId)
                                         .ModuleName;
        filiereStats.MaxMoyenne      = MaxRatedModule.AverageNotes;
        

        // 6) Persiste
        if (filiereStats.Id == 0)
            _db.StatistiquesFilieres.Add(filiereStats);
        else
            _db.StatistiquesFilieres.Update(filiereStats);

        await _db.SaveChangesAsync();
        return filiereStats;
    } 
    
    public async Task<double> CalculateAndStoreAverageRatingAsync(int FiliereId)
    {
        // 1) Récupérer les évaluations du module
        var evaluations = await _evalClient.GetByFiliereAsync(FiliereId);
        if (evaluations == null)
            throw new ArgumentNullException(nameof(evaluations), 
                $"Aucune évaluation trouvée pour filiere = {FiliereId}");

        var scores = evaluations.Select(e => e.Score).ToList();
        if (!scores.Any())
            throw new InvalidOperationException($"Pas de note pour moduleId = {FiliereId}");

        // 2) Calculer la moyenne
        double avg = scores.Average();

        // 3) Récupérer l'entité StatistiqueModule correspondante
        var statFiliere = await _db.StatistiquesFilieres
            .FirstOrDefaultAsync(sF => sF.FiliereId == FiliereId);

        if (statFiliere == null)
            throw new KeyNotFoundException(
                $"StatistiqueModule introuvable pour moduleId = {FiliereId}");

        // 4) Mettre à jour et persister
        statFiliere.AverageRating = avg; 
        await _db.SaveChangesAsync();

        // 5) Retourner la valeur calculée
        return avg;
    }   
    private double CalculateMedian(List<float> values)
    {
        var sorted = values.OrderBy(v => v).ToList();
        int mid   = sorted.Count / 2;
        return (sorted.Count % 2 == 0)
             ? (sorted[mid - 1] + sorted[mid]) / 2.0
             : sorted[mid];
    }

    private double CalculateStdDev(List<float> values)
    {
        var avg = values.Average();
        return Math.Sqrt(values.Average(v => Math.Pow(v - avg, 2)));
    }

    private double CalculateNps(List<float> values)
    {
        int promoters  = values.Count(v => v >= 4.5);
        int detractors = values.Count(v => v <= 3);
        int total      = values.Count;
        return 100.0 * promoters  / total
             - 100.0 * detractors / total;
    }
}    

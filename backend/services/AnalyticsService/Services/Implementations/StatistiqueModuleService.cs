using Microsoft.EntityFrameworkCore;

namespace AnalyticsService.Services.Implementations;
using AnalyticsService.Services.Interfaces; 
using AnalyticsService.ExternalClients.ClientInterfaces; 
using AnalyticsService.Models;
using AnalyticsService.ExternalClients.DTO; 
using AnalyticsService.Data;
public class StatistiqueModuleService : IStatistiqueModuleService<StatistiqueModule>
{
    private readonly IEvaluationClient _evalClient;
    private readonly IAnswerClient _ansClient;
    private readonly IQuestionClient _quesClient;
    private readonly IQuestionnaireClient _qClient; 
    private readonly INoteClient _noteClient;
    private readonly AnalyticsDbContext _db;
    public StatistiqueModuleService(
        IEvaluationClient evalClient,
        IAnswerClient ansClient,
        IQuestionClient quesClient,
        IQuestionnaireClient qClient,
        AnalyticsDbContext db, 
        INoteClient noteClient)
    {
        _evalClient = evalClient;
        _ansClient = ansClient;
        _quesClient = quesClient;
        _qClient = qClient;
        _db = db;
        _noteClient = noteClient;
    } 
    public async Task<StatistiqueModule> CreateAsync(int moduleId)
    {
        // On crée l’objet avec uniquement le ModuleId rempli
        var statistique = new StatistiqueModule
        {
            ModuleId = moduleId
            // Tous les autres champs restent à leur valeur par défaut (null ou 0)
        };

        _db.StatistiquesModules.Add(statistique);
        await _db.SaveChangesAsync();

        return statistique;
    }
    public async Task<StatistiqueModule> GetByPropertyAsync(int moduleId)
    {
        // On récupère la ligne stats pour le module
        var stats = await _db.StatistiquesModules
            .FirstOrDefaultAsync(s => s.ModuleId == moduleId);

        if (stats == null)
        {
            // Vous pouvez choisir de retourner null ou de lancer une exception
            throw new KeyNotFoundException(
                $"Aucune statistique trouvée pour ModuleId = {moduleId}");
        }

        return stats;
    }
    public async Task<StatistiqueModule> CalculateStandardStats(int moduleId)
    {
        var evaluations = await _evalClient.GetByModuleAsync(moduleId); 
        if (evaluations == null)
            throw new ArgumentNullException(nameof(evaluations));
        
        // On ignore les éventuels éléments null et on ne compte que les vrais éléments
        var evaluationDtos = evaluations.Where(e => e != null).ToList();
        

        // LINQ .Average()
        
    
        
                // 1. Récupérer les questionnaires et questions pour le module 
                var questionnaires = await _qClient.GetByModuleAsync(moduleId);
                var allQuestions = new List<QuestionDto>();
                foreach (var q in questionnaires)
                {
                    var questions = await _quesClient.GetByQuestionnaireAsync(q.QuestionnaireId);
                    allQuestions.AddRange(questions);
                }
    
                // 2. Récupérer toutes les réponses des questions
                var allAnswers = new List<AnswerDto>();
                foreach (var question in allQuestions)
                {
                    var answers = await _ansClient.GetByQuestionAsync(question.QuestionId);
                    allAnswers.AddRange(answers);
                }
    
                // 3. Filtrer les questions avec StatName non null
                var statQuestions = allQuestions.Where(q => !string.IsNullOrEmpty(q.StatName) && (q.StatName == "SatisfactionRate" || q.StatName == "NpsScore") ).ToList();
    
                // 4. Initialiser l'objet StatistiqueModule
                var stat = new StatistiqueModule { ModuleId = moduleId, CreatedAt = DateTime.UtcNow , AverageRating =  evaluationDtos.Average(e => e.Score)};
    
                // 5. Calculer les statistiques pour chaque question avec StatName
                foreach (var question in statQuestions)
                {
                    var values = allAnswers
                        .Where(a => a.QuestionId == question.QuestionId   && a.RatingAnswer.HasValue)
                        .Select(a => a.RatingAnswer.Value)
                        .ToList();
    
                    if (values.Any())
                    {    
                        double? statValue = question.StatName switch
                        {
                            "SatisfactionRate" => values.Average(),
                            
                            "NpsScore"=> CalculateNps(values),
                            _ => null 
                        };
    
                        // Remplir la propriété correspondante dans StatistiqueModule
                        switch (question.StatName)
                        {
                            
                                
                            case "SatisfactionRate": 
                                stat.SatisfactionRate = statValue ?? 0; 
                                break;
                            case "NpsScore":
                                stat.NpsScore = statValue;
                                break;
                            // Ajouter d'autres cas selon tes besoins
                        }
                    }
                }
                // ── 6) Sauvegarder l'objet StatistiqueModule dans la base de données
                _db.StatistiquesModules.Add(stat);  // Ajoute la nouvelle stat
                await _db.SaveChangesAsync();      // Persiste les changements dans la base
                return stat;
            }
    
    public async Task<double> CalculateAndStoreAverageRatingAsync(int moduleId)
    {
        // 1) Récupérer les évaluations du module
        var evaluations = await _evalClient.GetByModuleAsync(moduleId);
        if (evaluations == null)
            throw new ArgumentNullException(nameof(evaluations), 
                $"Aucune évaluation trouvée pour moduleId = {moduleId}");

        var scores = evaluations.Select(e => e.Score).ToList();
        if (!scores.Any())
            throw new InvalidOperationException($"Pas de note pour moduleId = {moduleId}");

        // 2) Calculer la moyenne
        double avg = scores.Average();

        // 3) Récupérer l'entité StatistiqueModule correspondante
        var statModule = await _db.StatistiquesModules
            .FirstOrDefaultAsync(sm => sm.ModuleId == moduleId);

        if (statModule == null)
            throw new KeyNotFoundException(
                $"StatistiqueModule introuvable pour moduleId = {moduleId}");

        // 4) Mettre à jour et persister
        statModule.AverageRating = avg; 
        await _db.SaveChangesAsync();

        // 5) Retourner la valeur calculée
        return avg;
    }   
    public async Task<StatistiqueModule> CalculateMarksStats(int moduleId)
    {
        // 1) Charger l’entité existan
        var stat = await _db.StatistiquesModules
            .FirstOrDefaultAsync(s => s.ModuleId == moduleId);
        if (stat == null)
            throw new KeyNotFoundException($"StatistiqueModule introuvable pour ModuleId={moduleId}");

        // 2) Récupérer les notes du module
        var notes = (await _noteClient.GetByModuleAsync(moduleId))?.ToList()
                    ?? new List<NoteDto>();

        // 3) Calculer les métriques
        if (notes.Any())
        {
            var scores = notes.Select(n => (double)n.Grade).OrderBy(x => x).ToList();

            stat.AverageNotes = Math.Round(scores.Average(), 2);
            stat.NoteMax      = scores.Max();
            stat.NoteMin      = scores.Min();
            stat.MedianNotes  = CalculateMedianNotes(notes);
            stat.StdevNotes   = CalculateStdDev(scores, stat.AverageNotes.Value);
            stat.PassRate     = CalculatePassRate(notes);
        }
        else
        {
            // Pas de notes : on met tout à 0
            stat.AverageNotes = 0;
            stat.NoteMax      = 0;
            stat.NoteMin      = 0;
            stat.MedianNotes  = 0;
            stat.StdevNotes   = 0;
            stat.PassRate     = 0;
        } 

      

        // 4) Enregistrer les changements
        await _db.SaveChangesAsync();
        return stat;
    } 
    private double CalculatePassRate(IEnumerable<NoteDto> notes  )
    {
        var total = notes.Count();
        if (total == 0) return 0;

        var passed = notes.Count(n =>
        {
            Console.WriteLine($"Grade: {n.Grade}");  // Ajoutez cette ligne pour vérifier les valeurs des notes
            return n.Grade >= 12.0;
        });
        return (double)(passed) / total * 100; 
    } 
     // Méthode de calcul de la médiane
            private double CalculateMedian(List<float> values)
            {
                var sortedValues = values.OrderBy(v => v).ToList();
                int mid = sortedValues.Count / 2;
                return sortedValues.Count % 2 == 0
                    ? (sortedValues[mid - 1] + sortedValues[mid]) / 2.0
                    : sortedValues[mid];
            }

            private double CalculateMedianNotes(List<NoteDto> notes)
            {
                if (notes == null)
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
                return sortedScores[mid];            }

            // Méthode de calcul de l'écart-type
            private double CalculateStdDev(List<double> values, double mean)
            {
                int n = values.Count;
                if (n <= 1) return 0;
                double sumSq = values.Sum(v => Math.Pow(v - mean, 2));
                return Math.Sqrt(sumSq / (n - 1));
            }
    
            // Méthode de calcul du NPS 
            private double CalculateNps(List<float> values)
            {
                var promoters = values.Count(v => v >= 4.5);
                var detractors = values.Count(v => v <= 3);
                return 100.0 * promoters / values.Count - 100.0 * detractors / values.Count;
            } 
}
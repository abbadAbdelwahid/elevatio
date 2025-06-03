namespace AnalyticsService.Services.Implementations;
using AnalyticsService.Services.Interfaces; 
using AnalyticsService.ExternalClients.ClientInterfaces; 
using AnalyticsService.Models;
using AnalyticsService.ExternalClients.DTO; 
using AnalyticsService.Data;
public class StatistiqueMFService : IStatistiqueMFService<StatistiqueModule>
{
    private readonly IEvaluationClient _evalClient;
    private readonly IAnswerClient _ansClient;
    private readonly IQuestionClient _quesClient;
    private readonly IQuestionnaireClient _qClient;
    private readonly AnalyticsDbContext _db;
    public StatistiqueMFService(
        IEvaluationClient evalClient,
        IAnswerClient ansClient,
        IQuestionClient quesClient,
        IQuestionnaireClient qClient,
        AnalyticsDbContext db)
    {
        _evalClient = evalClient;
        _ansClient = ansClient;
        _quesClient = quesClient;
        _qClient = qClient;
        _db = db;
    } 
    
    public async Task<StatistiqueModule> CalculateStats(int moduleId)
            {
                // 1. Récupérer les questionnaires et questions pour le module
                var questionnaires = await _qClient.GetByModuleAsync(moduleId);
                var allQuestions = new List<QuestionDto>();
                foreach (var q in questionnaires)
                {
                    var questions = await _quesClient.GetByQuestionnaireAsync(q.Id);
                    allQuestions.AddRange(questions);
                }
    
                // 2. Récupérer toutes les réponses des questions
                var allAnswers = new List<AnswerDto>();
                foreach (var question in allQuestions)
                {
                    var answers = await _ansClient.GetByQuestionAsync(question.Id);
                    allAnswers.AddRange(answers);
                }
    
                // 3. Filtrer les questions avec StatName non null
                var statQuestions = allQuestions.Where(q => !string.IsNullOrWhiteSpace(q.StatName)).ToList();
    
                // 4. Initialiser l'objet StatistiqueModule
                var stat = new StatistiqueModule { ModuleId = moduleId, CreatedAt = DateTime.UtcNow };
    
                // 5. Calculer les statistiques pour chaque question avec StatName
                foreach (var question in statQuestions)
                {
                    var values = allAnswers
                        .Where(a => a.QuestionId == question.Id)
                        .Select(a => a.AnswerValue)
                        .ToList();
    
                    if (values.Any())
                    {
                        double statValue = question.StatName switch
                        {
                            nameof(stat.AverageRating) => values.Average(),
                            nameof(stat.MedianRating) => CalculateMedian(values),
                            nameof(stat.StdDevRating) => CalculateStdDev(values),
                            nameof(stat.NpsScore) => CalculateNps(values),
                            _ => 0
                        };
    
                        // Remplir la propriété correspondante dans StatistiqueModule
                        switch (question.StatName)
                        {
                            case "AverageRating":
                                stat.AverageRating = statValue;
                                break;
                            case "MedianRating":
                                stat.MedianRating = statValue;
                                break;
                            case "StdDevRating":
                                stat.StdDevRating = statValue;
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
     // Méthode de calcul de la médiane
            private double CalculateMedian(List<int> values)
            {
                var sortedValues = values.OrderBy(v => v).ToList();
                int mid = sortedValues.Count / 2;
                return sortedValues.Count % 2 == 0
                    ? (sortedValues[mid - 1] + sortedValues[mid]) / 2.0
                    : sortedValues[mid];
            }
    
            // Méthode de calcul de l'écart-type
            private double CalculateStdDev(List<int> values)
            {
                var avg = values.Average();
                return Math.Sqrt(values.Average(v => Math.Pow(v - avg, 2)));
            }
    
            // Méthode de calcul du NPS
            private double CalculateNps(List<int> values)
            {
                var promoters = values.Count(v => v >= 9);
                var detractors = values.Count(v => v <= 6);
                return 100.0 * promoters / values.Count - 100.0 * detractors / values.Count;
            }
}
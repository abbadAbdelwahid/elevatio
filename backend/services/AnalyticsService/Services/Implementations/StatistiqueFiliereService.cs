namespace AnalyticsService.Services.Implementations;
using AnalyticsService.Services.Interfaces; 
using AnalyticsService.ExternalClients.ClientInterfaces; 
using AnalyticsService.Models;
using AnalyticsService.ExternalClients.DTO; 
using AnalyticsService.Data;

public class StatistiqueFiliereService:IStatistiqueService<StatistiqueFiliere>
{
private readonly IEvaluationClient _evalClient;
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

    public async Task<StatistiqueFiliere> CalculateStats(int filiereId)
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
                
                case "SatisfactionRate":
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

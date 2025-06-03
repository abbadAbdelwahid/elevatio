using AnalyticsService.Data;
using AnalyticsService.ExternalClients.DTO;
using AnalyticsService.ExternalClients.TestImplementations;
using AnalyticsService.Services.Implementations;
using Microsoft.EntityFrameworkCore; 
using Microsoft.Extensions.Configuration;

namespace AnalyticsService.AnalyticsTestApp;

public class Program
{
      static async Task Main()
        { 
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build(); 

            var connStr = config.GetConnectionString("AnalyticsDb");
            var options = new DbContextOptionsBuilder<AnalyticsDbContext>()
                .UseNpgsql(connStr)
                .Options;

            

// 2) Instanciez le contexte “à la main”
            await using var db = new AnalyticsDbContext(options);
            
            // 2) Préparez vos listes DTO
            var questionnaires = new[]
            {
                new QuestionnaireDto { Id=1, ModuleId=1, FiliereId=10, Title="Q1", Type="internal", CreatedAt=DateTime.UtcNow },
                new QuestionnaireDto { Id=2, ModuleId=3, FiliereId=10, Title="Q1", Type="internal", CreatedAt=DateTime.UtcNow }
            };
            var questions = new[]
            {
                new QuestionDto { Id=10, QuestionnaireId=1, QuestionText="Qualité", QuestionType="Scale", StatName="AverageRating", CreatedAt=DateTime.UtcNow },
                new QuestionDto { Id=11, QuestionnaireId=1, QuestionText="Écart-type", QuestionType="Scale", StatName="StdDevRating", CreatedAt=DateTime.UtcNow },
                new QuestionDto { Id=12, QuestionnaireId=1, QuestionText="Q", QuestionType="Scale", StatName="NpsScore", CreatedAt=DateTime.UtcNow }
            };
            var evaluations = new[]
            {
                new EvaluationDto { Id=100, ModuleId=1, Rating=2, CreatedAt=DateTime.UtcNow },
                new EvaluationDto { Id=101, ModuleId=1, Rating=5, CreatedAt=DateTime.UtcNow },
                new EvaluationDto { Id=102, ModuleId=1, Rating=3, CreatedAt=DateTime.UtcNow }
            };
            var answers = new[]
            {
                // 4 réponses pour Q10
                new AnswerDto{ Id=1, QuestionId=10, AnswerValue=3, CreatedAt=DateTime.UtcNow },
                new AnswerDto{ Id=2, QuestionId=10, AnswerValue=4, CreatedAt=DateTime.UtcNow },
                new AnswerDto{ Id=3, QuestionId=10, AnswerValue=2, CreatedAt=DateTime.UtcNow },
                new AnswerDto{ Id=4, QuestionId=10, AnswerValue=5, CreatedAt=DateTime.UtcNow },
                // 4 réponses pour Q11
                new AnswerDto{ Id=5, QuestionId=11, AnswerValue=1, CreatedAt=DateTime.UtcNow },
                new AnswerDto{ Id=6, QuestionId=11, AnswerValue=5, CreatedAt=DateTime.UtcNow },
                new AnswerDto{ Id=7, QuestionId=11, AnswerValue=3, CreatedAt=DateTime.UtcNow },
                new AnswerDto{ Id=8, QuestionId=11, AnswerValue=4, CreatedAt=DateTime.UtcNow },   
                // 4 réponses pour Q12
               
                new AnswerDto{ Id=5, QuestionId=12, AnswerValue=1, CreatedAt=DateTime.UtcNow },
                new AnswerDto{ Id=6, QuestionId=12, AnswerValue=5, CreatedAt=DateTime.UtcNow },
                new AnswerDto{ Id=7, QuestionId=12, AnswerValue=3, CreatedAt=DateTime.UtcNow },
                new AnswerDto{ Id=8, QuestionId=12, AnswerValue=4, CreatedAt=DateTime.UtcNow }
            };

            // 3) Instanciation des DummyClients
            var qClient = new DummyQuestionnaireClient(questionnaires);
            var qsClient = new DummyQuestionClient(questions);
            var evClient = new DummyEvaluationClient(evaluations);
            var ansClient = new DummyAnswerClient(answers);

            // 4) Instanciez le service
            var service = new StatistiqueModuleService(
                
                evClient,
                ansClient,
                qsClient, 
                qClient,
                db
            );

            // 5) Exécution : crée et persiste la statistique pour moduleId = 1
            var stat = await service.CalculateStats(1);

            // 6) Affichez le résultat
            Console.WriteLine($"AverageRating : {stat.AverageRating}");
            Console.WriteLine($"StdDevRating : {stat.StdDevRating}");

            // 7) Vérifiez la persistance en base
            var count = db.StatistiquesModules.Count();
            Console.WriteLine($"Enregistrements en base : {count}");
        }
}
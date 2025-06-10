using System.Net;
using System.Text;
using AnalyticsService.ExternalClients.DTO;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Layout;
using Microsoft.EntityFrameworkCore;
using MigraDocCore.DocumentObjectModel;
using PdfSharpCore.Drawing.Layout; 
using SelectPdf;


namespace AnalyticsService.Services.Implementations;
using AnalyticsService.Models;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using AnalyticsService.Services.Interfaces; 
using AnalyticsService.Data;
using System; 
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Collections.Generic; 
using AnalyticsService.ExternalClients.OpenAI; 
using AnalyticsService.ExternalClients.DTO;

using iText.Kernel.Pdf ; 
using iText.Kernel.Pdf;
using iText.Layout;
using DinkToPdf;
using DinkToPdf.Contracts;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks; 

using iText.Kernel.Geom ;  
using AnalyticsService.ExternalClients.ClientInterfaces;

public class ReportService : IReportService
{
    private readonly IGroqAIClient _groqAi;
    private readonly AnalyticsDbContext _db;
    private readonly IAnswerClient _ansClient;
    private readonly IQuestionClient _quesClient;
    private readonly IFiliereClient _filiereClient;
    private readonly IModuleClient _moduleClient; 
    private readonly IQuestionnaireClient _questionnaireClient;

    public ReportService(IGroqAIClient groqAi, AnalyticsDbContext db, IAnswerClient ansClient,
        IQuestionClient quesClient, IModuleClient moduleClient, IFiliereClient filiereClient)
    {
        _groqAi = groqAi;
        _db = db;
        _ansClient = ansClient;
        _quesClient = quesClient;
        _moduleClient = moduleClient;
        _filiereClient = filiereClient;
    }

    public async Task<string> GroqQuestionAnalyzer(QuestionDto q , IEnumerable<QuestionnaireDto> quest)
    {
        var answers = await _ansClient.GetByQuestionAsync(q.QuestionId);
        if (answers == null)
        {
            throw new InvalidOperationException($"Aucune réponse trouvée pour la question {q.QuestionId}.");
        } 
        
        // 2) Récupérer toutes les réponses pour chaque question
        var sb = new System.Text.StringBuilder(); 
        
        
           
        sb.AppendLine($"- voila la question {q.QuestionText} et son questionnaire {quest.FirstOrDefault()?.Title} dans le cadre d'une école d'ingénieur , je vais te donner les {answers.Count()} réponses sur cette question ");
        sb.AppendLine();
        sb.AppendLine(" rédige un résumé des résultats de cette question ");
        

        // 3) Appeler OpenAI
        var prompt = sb.ToString();  
        
        
        return await _groqAi.SendChatAsync(prompt);
    } 
    public  async Task<string> GroqAverageRating(double? averageRating, string NomModule)
    {
        // Créer le prompt pour GroqAI
        string prompt = $@"
    Le module '{NomModule}' a une moyenne de rating  de {averageRating}/5.  
Réponds moi strictement a ces questions dans une paragraphe  en prenant en cosidération CE {NomModule} et son contenu(ne mentionne pas prquoi tu genere ce message)
ne donne pas des reponses generales mais des reponses qui sont dans de corps de {NomModule} 

Comment ajuster les évaluations pour mieux refléter la progression des étudiants ?

Comment corriger les tendances de faibles performances (par exemple, revoir le contenu ou ajuster les évaluations) ?";

        // Utiliser GroqAI pour générer le texte du rapport
        var text = await _groqAi.SendChatAsync(prompt);

        return text;
    } 
    public  async Task<string> GroqMedianRating(double? medianRating, string NomModule)
    {
        // Créer le prompt pour GroqAI
        string prompt = $@" 
    Le module '{NomModule}' a une mediane des notes de {medianRating}/5.  
Réponds moi strictement a ces questions dans une paragraphe en prenant en cosidération CE {NomModule} et son contenu(ne mentionne pas prquoi tu genere ce message)
ne donne pas des reponses generales mais des reponses qui sont dans de corps de {NomModule} 
La médiane de {medianRating}/5 indique que la moitié des étudiants obtient au moins ce score. Quelles stratégies de remédiation ciblée proposeriez-vous pour aider ceux en-dessous de la médian ?
comment adapteriez-vous les supports (vidéos, fiches, tutoriels , prend ces elements en considération mais ne les mentionne pas directement)  pour homogénéiser les performances autour de ce point central ? "; 

        // Utiliser GroqAI pour générer le texte du rapport
        var text = await _groqAi.SendChatAsync(prompt);

        return text;
    } 
    public  async Task<string> GroqParticipationRate(double? PR, string NomModule)
    {
        // Créer le prompt pour GroqAI
        string prompt = $@"
    Le module '{NomModule}' a une participationRate  de {PR}/1.  
Réponds moi strictement a ces questions dans une paragraphe en prenant en cosidération CE {NomModule} et son contenu(ne mentionne pas prquoi tu genere ce message) 
ne donne pas des reponses generales mais des reponses qui sont dans de corps de {NomModule}  

Un taux de participation  révèle le degré d’engagement. Quels formats (quiz en direct, ateliers, forums) permettraient de faire évoluer ce taux à la hausse

 À {PR} de participation, quels obstacles (durée des séances, complexité, manque d’incitations) , prend ces element en considération mais ne les mentionne pas directement , semblent freiner les étudiants, et comment les lever ?";

        // Utiliser GroqAI pour générer le texte du rapport
        var text = await _groqAi.SendChatAsync(prompt);

        return text;
    } 
    public  async Task<string> GroqNpsScore(double? NPS, string NomModule)
    {
        // Créer le prompt pour GroqAI
        string prompt = $@"
    '{NomModule}' a une NpsScore des notes de {NPS}/100.  
Réponds moi strictement a ces questions dans une paragraphe  en prenant en cosidération CE {NomModule} et son contenu(ne mentionne pas prquoi tu genere ce message)
ne donne pas des reponses generales mais des reponses qui sont dans de corps de {NomModule} 
Avec ce NPS  , quels éléments spécifiques du module (rythme, clarté, interactivité) , prend ces element en considération mais ne les mentionne pas directement ,expliquent ce niveau de recommandation, et comment les renforcer ?

Le NPS  reflète la fidélité des étudiants : quelles modifications méthodologiques pourraient faire grimper ce score ? ?";
// Utiliser GroqAI pour générer le texte du rapport
        var text = await _groqAi.SendChatAsync(prompt);

        return text;
    } 
    public static byte[] GeneratePdf(string htmlContent)
    { 
        // Encapsuler le titre et le contenu dans un document HTML
        

        // Créer une instance de l'objet HtmlToPdf
        HtmlToPdf converter = new HtmlToPdf();
        SelectPdf.PdfDocument doc = null;

        try
        {
            // Convertir le contenu HTML en PDF
            doc = converter.ConvertHtmlString(htmlContent);

            // Sauvegarder le document PDF dans un MemoryStream
            using (var memoryStream = new MemoryStream())
            {
                doc.Save(memoryStream);
                doc.Close();
                
                // Retourner le tableau d'octets du PDF
                return memoryStream.ToArray();
            }
        }
        catch (Exception ex)
        {
            // Gérer les erreurs
            Console.WriteLine("Erreur lors de la génération du PDF : " + ex.Message);
            return null;
        }  
    
    }

    public async Task<byte[]> GenerateModuleReportPdfAsync(int moduleId)
    {
        try
        {
            // 1) Récupérer les statistiques du module
            var stat = await _db.StatistiquesModules.FindAsync(moduleId)
                       ?? throw new KeyNotFoundException("Module introuvable"); 
              

            ModuleDto module = await _moduleClient.GetModuleByIdAsync(moduleId);
            string ModuleName = module.ModuleName;
            string FiliereName = module.FiliereName; 
            string MF = $"{ModuleName} de la filiere {FiliereName}";
            
            var ResponseAVG = (stat.AverageRating != null && stat.AverageRating != 0) ? await GroqAverageRating(stat.AverageRating.Value, MF) : null;
            Console.Write(ResponseAVG);
// Créer le prompt pour la statistique "Médiane des notes"
            var ResponseMedian = (stat.MedianNotes != null && stat.MedianNotes != 0) ? await GroqMedianRating(stat.MedianNotes.Value, MF) : null;
// Créer le prompt pour la statistique "Taux de participation"
            Console.Write(ResponseMedian);
            var ResponseParticipation = (stat.ParticipationRate != null && stat.ParticipationRate != 0) ? await GroqParticipationRate(stat.ParticipationRate.Value, MF) : null;
// Créer le prompt pour la statistique "Net Promoter Score"
            Console.Write(ResponseParticipation);
            var ResponseNpsScore = (stat.NpsScore != null && stat.NpsScore != 0) ? await GroqNpsScore(stat.NpsScore.Value, MF) : null;
            Console.Write(ResponseNpsScore);
            // 2) Créer le prompt pour générer le rapport avec GroqAI
            var prompt =
                $@"Donne moi une conclusion générale de ces textes en une paragraphe : 
Texte1 : {ResponseAVG} 
Texte2 : {ResponseMedian}
Texte3 : {ResponseParticipation}
Texte4 : {ResponseNpsScore}";

            // 3) Générer le texte du rapport avec GroqAI
            var Conclusion = await _groqAi.SendChatAsync(prompt);

            // Vérifier si le texte généré est valide
            if (string.IsNullOrWhiteSpace(Conclusion))
            {
                throw new InvalidOperationException("Le texte du rapport généré est vide ou invalide.");
            }

            var htmlContent =
                $@"
<html>
    <head>
        <style>
            body {{
                font-family: Arial, sans-serif;
                font-size: 12pt;
                line-height: 1.6;
                margin: 20px;
            }}
            h1 {{
                text-align: center;
                color: #4CAF50;
                font-size: 24pt;
            }}
            h2 {{
                color: #333;
                font-size: 18pt;
            }}
            p {{
                margin-bottom: 15px;
            }}
        </style>
    </head>
    <body>
        <!-- Titre -->
        <h1>Rapport sur l'Évaluation du Module '{{ModuleName}}' de la Filière '{{FiliereName}}'</h1>

        <!-- Introduction -->
        <p><strong>Introduction :</strong><br>
        Ce rapport vise à analyser l'efficacité du {MF}. L'objectif est de comprendre les performances des étudiants en fonction des statistiques clés telles que la moyenne des notes, la médiane, le Net Promoter Score (NPS), et le taux de participation. Ce rapport fournit également des recommandations basées sur l'analyse de ces données.</p>

        <!-- Moyenne des Notes -->
        <p><strong>Moyenne des Notes : {stat.AverageRating}</strong><br>
       {ResponseAVG}</p>
        <!-- Médiane des Notes -->
        <p><strong>Médiane des Notes : {stat.MedianNotes}</strong><br>
     {ResponseMedian}</p>
        <!-- Net Promoter Score (NPS) -->
        <p><strong>Net Promoter Score (NPS) :{stat.NpsScore}</strong><br>
     {ResponseNpsScore}</p>
        <!-- Taux de Participation -->
        <p><strong>Taux de Participation :{stat.ParticipationRate}</strong><br>
       {ResponseParticipation}</p>

        <!-- Conclusion -->
        <p><strong>Conclusion :</strong><br>
      {Conclusion}</p>
    </body>
</html>
"; 
            Console.Write(Conclusion);
            Console.Write("------------------------------");
            Console.Write(htmlContent);
            // 4) Créer le PDF 
            return ReportService.GeneratePdf(htmlContent );


        }
        catch (KeyNotFoundException ex)
        {
            // Gérer le cas où le module n'est pas trouvé
            throw new Exception("Le module spécifié n'a pas été trouvé dans la base de données.", ex);
        }
        catch (InvalidOperationException ex)
        {
            // Gérer le cas où le texte généré est invalide
            throw new Exception("Erreur lors de la génération du rapport : " + ex.Message, ex);
        }

    }



public async Task<byte[]> GenerateFiliereReportPdfAsync(int filiereId)
{
   try
        {
            // 1) Récupérer les statistiques du module
            var stat = await _db.StatistiquesFilieres.FindAsync(filiereId)
                       ?? throw new KeyNotFoundException("Module introuvable"); 
              

            FiliereDto filiere = await _filiereClient.GetFiliereByIdAsync(filiereId);
         
            string FiliereName = filiere.FiliereName; 
           
            
            var ResponseAVG = (stat.AverageRating != null && stat.AverageRating != 0) ? await GroqAverageRating(stat.AverageRating.Value, FiliereName) : null;

// Créer le prompt pour la statistique "Médiane des notes"
            var ResponseMedian = (stat.MedianRating != null && stat.MedianRating != 0) ? await GroqMedianRating(stat.MedianRating.Value, FiliereName) : null;
// Créer le prompt pour la statistique "Taux de participation"
            var ResponseParticipation = (stat.SatisfactionRate!= null && stat.SatisfactionRate != 0) ? await GroqParticipationRate(stat.SatisfactionRate.Value, FiliereName) : null;
// Créer le prompt pour la statistique "Net Promoter Score"
            var ResponseNpsScore = (stat.NpsScore != null && stat.NpsScore != 0) ? await GroqNpsScore(stat.NpsScore.Value, FiliereName) : null;

            // 2) Créer le prompt pour générer le rapport avec GroqAI
            var prompt =
                $@"Donne moi une conclusion générale ou résumé sans répétion de ces textes en une paragraphe : 
{ResponseAVG} 
 
{ResponseMedian}

{ResponseParticipation}

{ResponseNpsScore}";

            // 3) Générer le texte du rapport avec GroqAI
            var Conclusion = await _groqAi.SendChatAsync(prompt);

            // Vérifier si le texte généré est valide
            if (string.IsNullOrWhiteSpace(Conclusion))
            {
                throw new InvalidOperationException("Le texte du rapport généré est vide ou invalide.");
            }

            var htmlContent =
                $@"
<html>
    <head>
        <style>
            body {{
                font-family: Arial, sans-serif;
                font-size: 12pt;
                line-height: 1.6;
                margin: 20px;
            }}
            h1 {{
                text-align: center;
                color: #4CAF50;
                font-size: 24pt;
            }}
            h2 {{
                color: #333;
                font-size: 18pt;
            }}
            p {{
                margin-bottom: 15px;
            }}
        </style>
    </head>
    <body>
        <!-- Titre -->
        <h1>Rapport sur l'Évaluation de la filière {FiliereName}'</h1>

        <!-- Introduction -->
        <p><strong>Introduction :</strong><br>
        Ce rapport vise à analyser l'efficacité du {FiliereName}. L'objectif est de comprendre les performances des étudiants en fonction des statistiques clés telles que la moyenne des notes, la médiane, le Net Promoter Score (NPS), et le taux de participation. Ce rapport fournit également des recommandations basées sur l'analyse de ces données.</p>

        <!-- Moyenne des Notes -->
        <p><strong>Moyenne des Notes : {stat.AverageRating}</strong><br>
       {ResponseAVG}</p>
        <!-- Médiane des Notes -->
        <p><strong>Médiane des Notes : {stat.MedianRating}</strong><br>
     {ResponseMedian}</p>
        <!-- Net Promoter Score (NPS) -->
        <p><strong>Net Promoter Score (NPS) :{stat.NpsScore}</strong><br>
     {ResponseNpsScore}</p>
        <!-- Taux de Participation -->
        <p><strong>Taux de Participation :{stat.SatisfactionRate}</strong><br>
       {ResponseParticipation}</p>

        <!-- Conclusion -->
        <p><strong>Conclusion :</strong><br>
      {Conclusion}</p>
    </body>
</html>
"; 
            Console.Write(Conclusion);
            
            // 4) Créer le PDF 
            return ReportService.GeneratePdf(htmlContent );

        }
        catch (KeyNotFoundException ex)
        {
            // Gérer le cas où le module n'est pas trouvé
            throw new Exception("Le module spécifié n'a pas été trouvé dans la base de données.", ex);
        }
        catch (InvalidOperationException ex)
        {
            // Gérer le cas où le texte généré est invalide
            throw new Exception("Erreur lors de la génération du rapport : " + ex.Message, ex);
        }

}

/*    public async Task<byte[]> GenerateEnsReportPdfAsync(int teacherId)
{
    try
    {
        // 1) Récupérer les statistiques de l'enseignant
        var stat = await _db.StatistiquesEnseignants.FindAsync(teacherId)
                   ?? throw new KeyNotFoundException("Enseignant introuvable");

        // 2) Créer le prompt pour générer le rapport avec GroqAI
        var prompt = $@"Rapport Enseignant {stat.TeacherId} (ISO 29990) :
- Moyenne générale    : {stat.AverageRating:F1}/5
- Moyenne modules     : {stat.AverageRatingM:F1}/5
- Participation        : {stat.ParticipationRate:P0}
- Feedback positif     : {stat.PositiveFeedbackPct:P0}
- Réponse moyenne      : {stat.ResponseTimeAvg:F1} h
- Tendance             : {stat.ImprovementTrend:+0.0;-0.0}%

Selon la norme ISO 29990, indique :
1) Points forts pédagogiques.
2) Axes d'amélioration.
3) Trois recommandations.";

        // 3) Générer le texte du rapport avec GroqAI
        var text = await _groqAi.SendChatAsync(prompt);

        // Vérifier si le texte généré est valide
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new InvalidOperationException("Le texte du rapport généré est vide ou invalide.");
        }

    
    }
    catch (KeyNotFoundException ex)
    {
        // Gérer le cas où l'enseignant n'est pas trouvé
        throw new Exception("L'enseignant spécifié n'a pas été trouvé dans la base de données.", ex);
    }
    catch (InvalidOperationException ex)
    {
        // Gérer le cas où le texte généré est invalide
        throw new Exception("Erreur lors de la génération du rapport : " + ex.Message, ex);
    }
    catch (Exception ex)
    {
        // Gérer d'autres exceptions générales
        throw new Exception("Une erreur est survenue lors de la génération du rapport PDF.", ex);
    }
}

  */   
   public async Task<byte[]> GenerateMQReportPdfAsync(int moduleId, int questionnaireId)
{
    try
    {   
        // 1) Récupérer les questions du questionnaire
        var questions = await _quesClient.GetByQuestionnaireAsync(questionnaireId); 
        // Si vous avez aussi besoin des métadonnées du questionnaire
        var questionnaire = await _questionnaireClient.GetByIdAsync(questionnaireId); 
        
        
        
        if (questions == null || !questions.Any())
        {
            throw new InvalidOperationException("Aucune question trouvée pour ce questionnaire.");
        }
        var sb = new StringBuilder(); 
        sb.AppendLine("<!DOCTYPE html>");
        sb.AppendLine("<html lang=\"fr\">");
        sb.AppendLine("<head>");
        sb.AppendLine("  <meta charset=\"UTF-8\" />");
        sb.AppendLine("  <title>Analyse Questionnaire</title>");
        sb.AppendLine("  <style>");
        sb.AppendLine("    body { font-family: Arial, sans-serif; margin: 20px; }");
        sb.AppendLine("    h2 { color: #2E86C1; }");
        sb.AppendLine("    p  { margin-bottom: 15px; }");
        sb.AppendLine("    hr { border: none; border-top: 1px solid #ccc; margin: 20px 0; }");
        sb.AppendLine("  </style>");
        sb.AppendLine("</head>");
        sb.AppendLine("<body>");
        foreach (QuestionDto question in questions)
        {
            // Titre de section
            sb.AppendLine($"<h2>Question : {WebUtility.HtmlEncode(question.QuestionText)}</h2>");
            sb.AppendLine("<hr/>");

            // Analyse IA
            string analysis = await GroqQuestionAnalyzer(question, questionnaire);
            sb.AppendLine($"<p>{WebUtility.HtmlEncode(analysis)}</p>");      
        } 
        // 3) Pied de page HTML
        sb.AppendLine("</body>");
        sb.AppendLine("</html>");

       String htmlContent =  sb.ToString();
       var pdfBytes = GeneratePdf(htmlContent); 
       
        var rapportMQ = new RapportMQ()
        {
            ModuleId = moduleId,
            QuestionnaireId = questionnaireId,
            RapportPdf = pdfBytes ,
            CreatedAt = DateTime.UtcNow
        }; 
        // 6) Enregistrer l'objet RapportFQ dans la base de données
        _db.RapportMQs.Add(rapportMQ); // _db est votre DbContext
        await _db.SaveChangesAsync();
        return pdfBytes;
    }
    catch (InvalidOperationException ex)
    {
        // Gestion des erreurs liées à la récupération des données ou au texte généré
        throw new Exception("Erreur lors de la génération du rapport : " + ex.Message, ex);
    }
    catch (Exception ex)
    {
        // Gestion des erreurs générales
        throw new Exception("Une erreur est survenue lors de la génération du rapport PDF.", ex);
    }
}

    public async Task<byte[]> GenerateFQReportPdfAsync(int filiereId, int questionnaireId)
{
    try
    {
        // 1) Récupérer les questions du questionnaire
        var questions = await _quesClient.GetByQuestionnaireAsync(questionnaireId);
        if (questions == null || !questions.Any())
        {
            throw new InvalidOperationException("Aucune question trouvée pour ce questionnaire.");
        }

        // 2) Récupérer toutes les réponses pour chaque question
        var sb = new System.Text.StringBuilder();
        sb.AppendLine($"Questionnaire {questionnaireId} de la filière {filiereId} :");
        foreach (var q in questions)
        {
            var answers = await _ansClient.GetByQuestionAsync(q.QuestionId);
            if (answers == null)
            {
                throw new InvalidOperationException($"Aucune réponse trouvée pour la question {q.QuestionId}.");
            }
            sb.AppendLine($"- Q{q.QuestionId}: {q.QuestionText} (Réponses: {answers.Count()})");
        }

        sb.AppendLine();
        sb.AppendLine("Conforme à ISO 21001, rédige :");
        sb.AppendLine("1) Synthèse des performances pédagogiques.");
        sb.AppendLine("2) Recommandations stratégiques pour la filière.");

        // 3) Appeler OpenAI pour générer le rapport
        var prompt = sb.ToString();
        var reportText = await _groqAi.SendChatAsync(prompt);

        // Vérification du texte généré
        if (string.IsNullOrWhiteSpace(reportText))
        {
            throw new InvalidOperationException("Le texte généré par OpenAI est vide ou invalide.");
        }

        // 4) Générer le PDF
        using var ms = new MemoryStream();
        var writer = new PdfWriter(ms);
        var pdfDoc = new iText.Kernel.Pdf.PdfDocument(writer);
        var doc = new Document(pdfDoc, PageSize.A4);  
        doc.SetMargins(40, 40, 40, 40);

        doc.Add(new Paragraph($"Rapport Questionnaire {questionnaireId} - Filière {filiereId}")
            .SetFontSize(18)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetMarginBottom(10));

        doc.Add(new Paragraph(reportText)
            .SetFontSize(12)
            .SetTextAlignment(TextAlignment.JUSTIFIED));

        doc.Close();
        var pdfBytes = ms.ToArray(); 
        var rapportFQ = new RapportFQ
        {
            FiliereId = filiereId,
            QuestionnaireId = questionnaireId,
            RapportPdf = pdfBytes,
            CreatedAt = DateTime.UtcNow
        }; 
        // 6) Enregistrer l'objet RapportFQ dans la base de données
        _db.RapportFQs.Add(rapportFQ); // _db est votre DbContext
        await _db.SaveChangesAsync();
        return pdfBytes;
    }
    catch (InvalidOperationException ex)
    {
        // Gestion des erreurs liées à la récupération des données ou au texte généré
        throw new Exception("Erreur lors de la génération du rapport : " + ex.Message, ex);
    }
    catch (Exception ex)
    {
        // Gestion des erreurs générales
        throw new Exception("Une erreur est survenue lors de la génération du rapport PDF.", ex);
    }
}


}
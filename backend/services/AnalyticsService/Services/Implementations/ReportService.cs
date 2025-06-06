using iText.Layout;
using Microsoft.EntityFrameworkCore;

namespace AnalyticsService.Services.Implementations;
using AnalyticsService.Models;
using AnalyticsService.Services.Interfaces; 
using AnalyticsService.Data;
using System;
using System.Collections.Generic; 
using AnalyticsService.ExternalClients.OpenAI; 
using iText.Kernel.Pdf ; 
using iText.Kernel.Pdf;
using iText.Layout;
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
     private readonly IGroqAIClient      _groqAi;
    private readonly AnalyticsDbContext _db; 
    private readonly IAnswerClient _ansClient;
    private readonly IQuestionClient _quesClient;

    public ReportService(IGroqAIClient groqAi, AnalyticsDbContext db , IAnswerClient ansClient, IQuestionClient quesClient)
    {
        _groqAi = groqAi;
        _db     = db; 
        _ansClient = ansClient;
        _quesClient = quesClient;
    }

    public async Task<byte[]> GenerateModuleReportPdfAsync(int moduleId )
{
    try
    {
        // 1) Récupérer les statistiques du module
        var stat = await _db.StatistiquesModules.FindAsync(moduleId)
                   ?? throw new KeyNotFoundException("Module introuvable");

        // 2) Créer le prompt pour générer le rapport avec GroqAI
        var prompt = $@"Rapport Module {stat.ModuleId} :
- Moyenne              : {stat.AverageRating:F1}/5
- Taux de participation: {stat.ParticipationRate:P0}
- Feedback positif     : {stat.PositiveFeedbackPct:P0}
- Tendance             : {stat.ImprovementTrend:+0.0;-0.0}%

En t'appuyant sur la norme ISO9001, fais :
1) Synthèse des points clés.
2) Trois recommandations stratégiques.";

        // 3) Générer le texte du rapport avec GroqAI
        var text = await _groqAi.SendChatAsync(prompt);
        
        // Vérifier si le texte généré est valide
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new InvalidOperationException("Le texte du rapport généré est vide ou invalide.");
        }
        var response = new
        {
            MessageSent = prompt,        // Message envoyé à GroqAI
            GeneratedText = text // Réponse générée par GroqAI
        };
        // 4) Créer le PDF
        using var ms = new MemoryStream();
        var writer = new PdfWriter(ms);
        var pdfDoc = new PdfDocument(writer);
        var doc = new Document(pdfDoc, PageSize.A4);
        doc.SetMargins(40, 40, 40, 40);

        // Ajouter le titre du rapport
        doc.Add(new Paragraph("Rapport Module " + stat.ModuleId)
            .SetFontSize(18)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetMarginBottom(15));

        // Ajouter le texte généré dans le PDF
        doc.Add(new Paragraph(text)
            .SetFontSize(12)
            .SetTextAlignment(TextAlignment.JUSTIFIED));

        doc.Close();

        // Convertir le PDF en tableau de bytes
       var pdfBytes = ms.ToArray();

        // 5) Stocker le PDF dans la base de données
        stat.RapportPdf = pdfBytes;
        stat.CreatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return pdfBytes;
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
    catch (Exception ex)
    {
        // Gérer d'autres exceptions générales
        throw new Exception("Une erreur est survenue lors de la génération du rapport PDF.", ex);
    }
}


   public async Task<byte[]> GenerateFiliereReportPdfAsync(int filiereId)
{
    try
    {
        // 1) Récupérer les statistiques de la filière
        var stat = await _db.StatistiquesFilieres.FirstOrDefaultAsync(s => s.FiliereId == filiereId)
                   ?? throw new KeyNotFoundException("Filière introuvable");

        // 2) Créer le prompt pour générer le rapport avec GroqAI
        var prompt = $@"Rapport Filière {stat.FiliereId} (ISO 21001) :
_Median Rating : {stat.MedianRating:F1}
- Participation         : {stat.ParticipationRate:P0}
- NPS                  : {stat.NpsScore:F1}
- Plans d'action       : {stat.ActionPlanCount}
- Tendance             : {stat.ImprovementTrend:+0.0;-0.0}%

En t'appuyant sur la norme ISO 9001, fais :

1) Synthèse des points clés.
   - Utilise les indicateurs d'évaluation médiane, participation, NPS, plans d'action, et tendance pour analyser la performance de la filière en tant que programme d'ingénierie.
   - Considère les spécificités d'une école d'ingénieurs dans l'évaluation de la qualité pédagogique, de l'implication des étudiants et de la préparation au marché du travail.
   - Mentionne des critères d'innovation et de recherche appliquée propres aux filières d'ingénierie.

2) Trois recommandations stratégiques :
   - Fais des recommandations spécifiques à l'amélioration continue dans un cadre d'enseignement technique supérieur.
   - Propose des actions concrètes pour augmenter l'engagement des étudiants dans les formations pratiques, les stages et la recherche.
   - Intègre des suggestions pour renforcer la collaboration avec les entreprises et améliorer la pertinence des plans d'action pour chaque domaine technique.

";

        // 3) Générer le texte du rapport avec GroqAI
        var text = await _groqAi.SendChatAsync(prompt);
        var response = new
        {
            MessageSent = prompt,
            GeneratedText = text
        }; 
        // Vérifier si le texte généré est valide
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new InvalidOperationException("Le texte du rapport généré est vide ou invalide.");
        }
        Console.Write(response); 
        // 4) Créer le PDF
        using var ms = new MemoryStream();
        var writer = new PdfWriter(ms);
        var pdfDoc = new PdfDocument(writer);
        var doc = new Document(pdfDoc, PageSize.A4);
        doc.SetMargins(40, 40, 40, 40);

        // Ajouter le titre du rapport
        doc.Add(new Paragraph("Rapport Filière " + stat.FiliereId)
            .SetFontSize(18)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetMarginBottom(15));

        // Ajouter le texte généré dans le PDF
        doc.Add(new Paragraph(text)
            .SetFontSize(12)
            .SetTextAlignment(TextAlignment.JUSTIFIED));

        doc.Close();

        // Convertir le PDF en tableau de bytes
        var pdfBytes = ms.ToArray();

        // 5) Stocker le PDF dans la base de données
        stat.RapportPdf = pdfBytes;
        stat.CreatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        
         return pdfBytes; 
         
    }
    catch (KeyNotFoundException ex) 
    {
        // Gérer le cas où la filière n'est pas trouvée
        throw new Exception("La filière spécifiée n'a pas été trouvée dans la base de données.", ex);
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

    public async Task<byte[]> GenerateEnsReportPdfAsync(int teacherId)
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

        // 4) Créer le PDF
        using var ms = new MemoryStream();
        var writer = new PdfWriter(ms);
        var pdfDoc = new PdfDocument(writer);
        var doc = new Document(pdfDoc, PageSize.A4);
        doc.SetMargins(40, 40, 40, 40);

        // Ajouter le titre du rapport
        doc.Add(new Paragraph("Rapport Enseignant " + stat.TeacherId)
            .SetFontSize(18)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetMarginBottom(15));

        // Ajouter le texte généré dans le PDF
        doc.Add(new Paragraph(text)
            .SetFontSize(12)
            .SetTextAlignment(TextAlignment.JUSTIFIED));

        doc.Close();

        // Convertir le PDF en tableau de bytes
        var pdfBytes = ms.ToArray();

        // 5) Stocker le PDF dans la base de données
        stat.RapportPdf = pdfBytes;
        stat.CreatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return pdfBytes;
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

    
   public async Task<byte[]> GenerateMQReportPdfAsync(int moduleId, int questionnaireId)
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
        sb.AppendLine($"Questionnaire {questionnaireId} du module {moduleId} :");
        foreach(var q in questions)
        {
            var answers = await _ansClient.GetByQuestionAsync(q.QuestionId);
            if (answers == null)
            {
                throw new InvalidOperationException($"Aucune réponse trouvée pour la question {q.QuestionId}.");
            }
            sb.AppendLine($"- Q{q.QuestionId}: {q.QuestionText} (Réponses: {answers.Count()})");
        }
        sb.AppendLine();
        sb.AppendLine("En te basant sur la norme ISO 9001, rédige un rapport détaillé des résultats du questionnaire, incluant :");
        sb.AppendLine("1) Synthèse des points saillants par question.");
        sb.AppendLine("2) Analyse des tendances et recommandations d'amélioration.");

        // 3) Appeler OpenAI
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
        var pdfDoc = new PdfDocument(writer);
        var doc = new Document(pdfDoc, PageSize.A4);
        doc.SetMargins(40, 40, 40, 40);

        doc.Add(new Paragraph($"Rapport Questionnaire {questionnaireId}")
            .SetFontSize(18)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetMarginBottom(10));

        doc.Add(new Paragraph(reportText)
            .SetFontSize(12)
            .SetTextAlignment(TextAlignment.JUSTIFIED));

        doc.Close();

        var pdfBytes = ms.ToArray(); 
        var rapportMQ = new RapportMQ()
        {
            ModuleId = moduleId,
            QuestionnaireId = questionnaireId,
            RapportPdf = pdfBytes,
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
        var pdfDoc = new PdfDocument(writer);
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
using iText.Layout;

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
     private readonly IOpenAIClient      _openAi;
    private readonly AnalyticsDbContext _db; 
    private readonly IAnswerClient _ansClient;
    private readonly IQuestionClient _quesClient;

    public ReportService(IOpenAIClient openAi, AnalyticsDbContext db , IAnswerClient ansClient, IQuestionClient quesClient)
    {
        _openAi = openAi;
        _db     = db; 
        _ansClient = ansClient;
        _quesClient = quesClient;
    }

    public async Task<byte[]> GenerateModuleReportPdfAsync(int moduleId)
    {
        var stat = await _db.StatistiquesModules.FindAsync(moduleId)
                   ?? throw new KeyNotFoundException("Module introuvable");
        var prompt = $@"Rapport Module {stat.ModuleId} :
- Moyenne              : {stat.AverageRating:F1}/5
- Taux de participation: {stat.ParticipationRate:P0}
- Feedback positif     : {stat.PositiveFeedbackPct:P0}
- Tendance             : {stat.ImprovementTrend:+0.0;-0.0}%

Rédige un résumé des points forts/faibles et trois recommandations.";
        // 1) Générer le texte
        var text = await _openAi.SendChatAsync(prompt);
        
        // 2) Construire le PDF
        using var ms    = new MemoryStream();
        var writer      = new PdfWriter(ms);
        var pdfDoc      = new PdfDocument(writer);
        var doc         = new Document(pdfDoc, PageSize.A4);
        doc.SetMargins(40,40,40,40);

        doc.Add(new Paragraph("Rapport Module " + stat.ModuleId)
            .SetFontSize(18)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetMarginBottom(15));

        doc.Add(new Paragraph(text)
            .SetFontSize(12)
            .SetTextAlignment(TextAlignment.JUSTIFIED));

        doc.Close();
        var pdfBytes = ms.ToArray();

        // 3) Stocker le PDF
        stat.RapportPdf = pdfBytes;
        stat.CreatedAt  = DateTime.UtcNow;
        await _db.SaveChangesAsync();
       
        return pdfBytes;
    }

    public async Task<byte[]> GenerateFiliereReportPdfAsync(int filiereId)
    {
        var stat = await _db.StatistiquesFilieres.FindAsync(filiereId)
                   ?? throw new KeyNotFoundException("Filière introuvable");
        var prompt = $@"Rapport Filière {stat.FiliereId} (ISO 21001) :
- Satisfaction moyenne : {stat.AverageRating:F1}/5
- Participation         : {stat.ParticipationRate:P0}
- NPS                  : {stat.NpsScore:F1}
- Plans d'action       : {stat.ActionPlanCount}
- Tendance             : {stat.ImprovementTrend:+0.0;-0.0}%

En t'appuyant sur la norme ISO 9001, fais :
1) Synthèse des points clés.
2) Trois recommandations stratégiques.";
        var text = await _openAi.SendChatAsync(prompt);
        using var ms    = new MemoryStream();
        var writer      = new PdfWriter(ms);
        var pdfDoc      = new PdfDocument(writer);
        var doc         = new Document(pdfDoc, PageSize.A4);
        doc.SetMargins(40,40,40,40);

        doc.Add(new Paragraph("Rapport Filière " + stat.FiliereId)
            .SetFontSize(18)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetMarginBottom(15));

        doc.Add(new Paragraph(text)
            .SetFontSize(12)
            .SetTextAlignment(TextAlignment.JUSTIFIED));

        doc.Close();
        var pdfBytes = ms.ToArray();

        stat.RapportPdf = pdfBytes;
        stat.CreatedAt  = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return pdfBytes;
    }

    public async Task<byte[]> GenerateEnsReportPdfAsync(int teacherId)
    {
        var stat = await _db.StatistiquesEnseignants.FindAsync(teacherId)
                   ?? throw new KeyNotFoundException("Enseignant introuvable");
        var prompt = $@"Rapport Enseignant {stat.TeacherId} (ISO 29990) :
- Moyenne générale    : {stat.AverageRating:F1}/5
- Moyenne modules     : {stat.AverageRatingM:F1}/5
- Participation        : {stat.ParticipationRate:P0}
- Feedback positif     : {stat.PositiveFeedbackPct:P0}
- Réponse moyenne      : {stat.ResponseTimeAvg:F1} h
- Tendance             : {stat.ImprovementTrend:+0.0;-0.0}%

Selon la norme ISO 29990, indique :
1) Points forts pédagogiques.
2) Axes d'amélioration.
3) Trois recommandations.";

        var text = await _openAi.SendChatAsync(prompt);
        using var ms = new MemoryStream();
        var writer = new PdfWriter(ms);
        var pdfDoc = new PdfDocument(writer);
        var doc = new Document(pdfDoc, PageSize.A4);
        doc.SetMargins(40, 40, 40, 40);

        doc.Add(new Paragraph("Rapport Filière " + stat.TeacherId)
            .SetFontSize(18)

            .SetTextAlignment(TextAlignment.CENTER)
            .SetMarginBottom(15));

        doc.Add(new Paragraph(text)
            .SetFontSize(12)
            .SetTextAlignment(TextAlignment.JUSTIFIED));

        doc.Close();
        var pdfBytes = ms.ToArray();

        stat.RapportPdf = pdfBytes;
        stat.CreatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return pdfBytes;
    }   
    
    public async Task<byte[]> GenerateMQReportPdfAsync(int moduleId, int questionnaireId)
    {
        // 1) Récupérer les questions du questionnaire
        var questions = await _quesClient.GetByQuestionnaireAsync(questionnaireId);
        // 2) Récupérer toutes les réponses pour chaque question
        var sb = new System.Text.StringBuilder();
        sb.AppendLine($"Questionnaire {questionnaireId} du module {moduleId} :");
        foreach(var q in questions)
        {
            var answers = await _ansClient.GetByQuestionAsync(q.Id);
            sb.AppendLine($"- Q{q.Id}: {q.QuestionText} (Réponses: {answers.Count()})");
        }
        sb.AppendLine();
        sb.AppendLine("En te basant sur la norme ISO 9001, rédige un rapport détaillé des résultats du questionnaire, incluant :");
        sb.AppendLine("1) Synthèse des points saillants par question.");
        sb.AppendLine("2) Analyse des tendances et recommandations d'amélioration.");

        // 3) Appeler OpenAI
        var prompt = sb.ToString();
        var reportText = await _openAi.SendChatAsync(prompt);

        // 4) Générer le PDF
        using var ms    = new MemoryStream();
        var writer      = new PdfWriter(ms);
        var pdfDoc      = new PdfDocument(writer);
        var doc         = new Document(pdfDoc, PageSize.A4);
        doc.SetMargins(40,40,40,40);

        doc.Add(new Paragraph($"Rapport Questionnaire {questionnaireId}")
            .SetFontSize(18).SetTextAlignment(TextAlignment.CENTER)
            .SetMarginBottom(10));
        doc.Add(new Paragraph(reportText)
            .SetFontSize(12).SetTextAlignment(TextAlignment.JUSTIFIED));
        doc.Close();

        return ms.ToArray();
    } 
    public async Task<byte[]> GenerateFQReportPdfAsync(int filiereId, int questionnaireId)
    {
        // Même logique que module, adaptée à la filière
        var questions = await _quesClient.GetByQuestionnaireAsync(questionnaireId);
        var sb = new System.Text.StringBuilder();
        sb.AppendLine($"Questionnaire {questionnaireId} de la filière {filiereId} :");
        foreach(var q in questions)
        {
            var answers = await _ansClient.GetByQuestionAsync(q.Id);
            sb.AppendLine($"- Q{q.Id}: {q.QuestionText} (Réponses: {answers.Count()})");
        }
        sb.AppendLine();
        sb.AppendLine("Conforme à ISO 21001, rédige :");
        sb.AppendLine("1) Synthèse des performances pédagogiques.");
        sb.AppendLine("2) Recommandations stratégiques pour la filière.");

        var prompt = sb.ToString();
        var reportText = await _openAi.SendChatAsync(prompt);

        using var ms    = new MemoryStream();
        var writer      = new PdfWriter(ms);
        var pdfDoc      = new PdfDocument(writer);
        var doc         = new Document(pdfDoc, PageSize.A4);
        doc.SetMargins(40,40,40,40);

        doc.Add(new Paragraph($"Rapport Questionnaire {questionnaireId} - Filière {filiereId}")
            .SetFontSize(18).SetTextAlignment(TextAlignment.CENTER)
            .SetMarginBottom(10));
        doc.Add(new Paragraph(reportText)
            .SetFontSize(12).SetTextAlignment(TextAlignment.JUSTIFIED));
        doc.Close();

        return ms.ToArray();
    }


}
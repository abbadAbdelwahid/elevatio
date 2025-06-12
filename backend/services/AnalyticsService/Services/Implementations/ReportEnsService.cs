using AnalyticsService.Data;
using AnalyticsService.ExternalClients.ClientInterfaces;
using AnalyticsService.ExternalClients.OpenAI;
using AnalyticsService.Services.Interfaces;
using Microsoft.EntityFrameworkCore; 
using System.Diagnostics;
using System.IO;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using SelectPdf;

namespace AnalyticsService.Services.Implementations;

public class ReportEnsService : IReportUserService
{
    private readonly IGroqAIClient _groqAi;
    private readonly AnalyticsDbContext _db;
    private readonly IAnswerClient _ansClient;
    private readonly IQuestionClient _quesClient;
    private readonly IFiliereClient _filiereClient;
    private readonly IModuleClient _moduleClient;
    private readonly IQuestionnaireClient _questionnaireClient;

    public ReportEnsService(IGroqAIClient groqAi, AnalyticsDbContext db, IAnswerClient ansClient,
        IQuestionClient quesClient, IModuleClient moduleClient, IFiliereClient filiereClient)
    {
        _groqAi = groqAi;
        _db = db;
        _ansClient = ansClient;
        _quesClient = quesClient;
        _moduleClient = moduleClient;
        _filiereClient = filiereClient;
    }
   
public static byte[] GeneratePdf(string htmlContent)
    { // Encapsuler le titre et le contenu dans un document HTML
        

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
    public async Task<string> PassAvgAnalyse(double? avg, double? pass)
    {

        // Créer le prompt pour l'analyse des moyennes des modules et du taux de validation, et les recommandations
        var prompt = $@"
    Ces stats sont d'un enseignant en ecole d'ingénieur:
    - Moyenne des modules : {avg:F1}/5
    - Taux de validation : {pass:F1}%

    Cet enseignant enseigne dans une école d'ingénieur.

   fais moi une analyse 
   en interprétant comment l'un peut affecter l'autre stat et fais des recommandations pour que les stats soient conformes ";
        // Générer la réponse via GroqAI
        var response = await _groqAi.SendChatAsync(prompt);
        return response; 

    }

    public async Task<byte[]> GenerateUserPerformanceReport(int teacherId)
    {
        try
        {
            // 1) Récupérer les statistiques de l'enseignant
            var stat = await _db.StatistiquesEnseignants.FirstOrDefaultAsync(s => s.TeacherId == teacherId)
                       ?? throw new KeyNotFoundException("Enseignant introuvable");
            
            var Response = (stat.AverageM != null && stat.PassRate != 0)
                ? await PassAvgAnalyse(stat.AverageM, stat.PassRate)
                : null;
            Console.Write(Response);


            string htmlContent = $@"
    
<html>
        <head>
            <title>Rapport de Performance de l'Enseignant</title>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    line-height: 1.6;
                }}
                h1 {{
                    color: #2C3E50;
                    text-align: center;
                }}
                .section {{
                    margin: 20px 0;
                    padding: 10px;
                    background-color: #F4F6F7;
                    border-radius: 8px;
                }}
                .section h2 {{
                    color: #34495E;
                }}
                .recommendations {{
                    background-color: #E8F8F5;
                    padding: 10px;
                    border-radius: 8px;
                    margin-top: 10px;
                }}
                .analysis {{
                    background-color: #FFFAF0;
                    padding: 10px;
                    border-radius: 8px;
                    margin-top: 10px;
                }}
            </style>
        </head>
        <body>
            <h1>Rapport de Performance de l'Enseignant</h1>

            <div class='section'>
                <h2>Analyse des Performances</h2>
                <div class='analysis'>
                    <p>{Response}</p>
                </div>
            </div>

            <div class='section'>
                <h2>Recommandations</h2>
                <div class='recommendations'>
                    <p>Basé sur l'analyse ci-dessus, voici les recommandations adaptées :</p>
                    <ul>
                        <li>Améliorer la préparation des étudiants avant les évaluations.</li>
                        <li>Réviser les critères d'évaluation pour garantir des résultats plus équitables.</li>
                        <li>Fournir davantage de ressources pédagogiques pour les étudiants en difficulté.</li>
                    </ul>
                </div>
            </div>

            <div class='section'>
                <h2>Conclusion</h2>
                <p>Ce rapport présente une analyse détaillée des statistiques de l'enseignant et propose des recommandations pratiques pour améliorer l'enseignement et la réussite des étudiants.</p>
            </div>
        </body>
    </html>";
            var pdfBytes =  ReportEnsService.GeneratePdf(htmlContent);
           
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

    public async Task<string> GenerateUserPerformanceReporthtml(int UserId)
    {
          try 
        {
            // 1) Récupérer les statistiques de l'enseignant
            var stat = await _db.StatistiquesEnseignants.FirstOrDefaultAsync(s => s.TeacherId == UserId)
                       ?? throw new KeyNotFoundException("Enseignant introuvable");
            
            var Response = (stat.AverageM != null && stat.PassRate != 0)
                ? await PassAvgAnalyse(stat.AverageM, stat.PassRate)
                : null;
            Console.Write(Response);


            string htmlContent = $@"
    
<html>
        <head>
            <title>Rapport de Performance de l'Enseignant</title>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    line-height: 1.6;
                }}
                h1 {{
                    color: #2C3E50;
                    text-align: center;
                }}
                .section {{
                    margin: 20px 0;
                    padding: 10px;
                    background-color: #F4F6F7;
                    border-radius: 8px;
                }}
                .section h2 {{
                    color: #34495E;
                }}
                .recommendations {{
                    background-color: #E8F8F5;
                    padding: 10px;
                    border-radius: 8px;
                    margin-top: 10px;
                }}
                .analysis {{
                    background-color: #FFFAF0;
                    padding: 10px;
                    border-radius: 8px;
                    margin-top: 10px;
                }}
            </style>
        </head>
        <body>
            <h1>Rapport de Performance de l'Enseignant</h1>

            <div class='section'>
                <h2>Analyse des Performances</h2>
                <div class='analysis'>
                    <p>{Response}</p>
                </div>
            </div>

            <div class='section'>
                <h2>Recommandations</h2>
                <div class='recommendations'>
                    <p>Basé sur l'analyse ci-dessus, voici les recommandations adaptées :</p>
                    <ul>
                        <li>Améliorer la préparation des étudiants avant les évaluations.</li>
                        <li>Réviser les critères d'évaluation pour garantir des résultats plus équitables.</li>
                        <li>Fournir davantage de ressources pédagogiques pour les étudiants en difficulté.</li>
                    </ul>
                </div>
            </div>

            <div class='section'>
                <h2>Conclusion</h2>
                <p>Ce rapport présente une analyse détaillée des statistiques de l'enseignant et propose des recommandations pratiques pour améliorer l'enseignement et la réussite des étudiants.</p>
            </div>
        </body>
    </html>";
            
           
            return htmlContent;

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

    }
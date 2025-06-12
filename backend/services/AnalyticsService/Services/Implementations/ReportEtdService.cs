using AnalyticsService.Data;
using AnalyticsService.ExternalClients.ClientInterfaces;
using AnalyticsService.ExternalClients.OpenAI;
using AnalyticsService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using SelectPdf;

namespace AnalyticsService.Services.Implementations;

public class ReportEtdService : IReportEtdService
{ 
    private readonly IGroqAIClient _groqAi;
    private readonly AnalyticsDbContext _db;
    private readonly IAnswerClient _ansClient;
    private readonly IQuestionClient _quesClient;
    private readonly IFiliereClient _filiereClient;
    private readonly IModuleClient _moduleClient; 
    private readonly IQuestionnaireClient _questionnaireClient;  
    private readonly INoteClient _noteClient;  

    
    public ReportEtdService(IGroqAIClient groqAi, AnalyticsDbContext db, IAnswerClient ansClient,
        IQuestionClient quesClient, IModuleClient moduleClient, IFiliereClient filiereClient,INoteClient noteClient)
    {
        _groqAi = groqAi;
        _db = db;
        _ansClient = ansClient;
        _quesClient = quesClient;
        _moduleClient = moduleClient;
        _filiereClient = filiereClient;
        _noteClient = noteClient;
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
    public async Task<string> EtdAnalyse(double? avg, double? pass , double? NoteMax , double? NoteMin )
    {

        // Créer le prompt pour l'analyse des moyennes des modules et du taux de validation, et les recommandations
        var prompt = $@"
    Ces stats sont d'un etudiant  en ecole d'ingénieur:
    - Moyenne  : {avg:F1}/20
    - Taux de validation : {pass:F1}% 
-Note max : {NoteMax:F1}/20 
-Note min: {NoteMin:F1}/20 
fais moi un analyse de ces donneees ainsi que des conseils afin d'améliorer son niveau, répond directement sans introduction

   "; 
        var response = await _groqAi.SendChatAsync(prompt);
        return response; 

    } 
  
    
     public async Task<byte[]> GenerateUserPerformanceReport(int etdId)
    {
        try
        {
            // 1) Récupérer les statistiques de l'enseignant
            var stat = await _db.StatistiquesEtudiants.FirstOrDefaultAsync(s => s.StudentId== etdId)
                       ?? throw new KeyNotFoundException("Etudiant not found");
            
            var Response = (stat.NoteMoyenne != null && stat.NoteMax != null && stat.NoteMin != null && stat.PassRate != null)
                ? await EtdAnalyse(stat.NoteMoyenne,stat.PassRate, stat.NoteMax,stat.NoteMin)
                : null;
            Console.Write(Response); 
            


            string htmlContent = $@"
    
<html>
        <head>
            <title>Rapport de Performance de l'Etudiant </title>
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
            <h1>Rapport de Performance de l'Etudiant</h1>

            <div class='section'>
                <h2>Analyse des Performances</h2>
                <div class='analysis'>
                    <p>{Response}</p>
                </div>
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
}
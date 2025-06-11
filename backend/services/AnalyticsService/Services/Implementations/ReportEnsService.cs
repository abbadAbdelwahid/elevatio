using AnalyticsService.Data;
using AnalyticsService.ExternalClients.ClientInterfaces;
using AnalyticsService.ExternalClients.OpenAI;
using AnalyticsService.Services.Interfaces;
using SelectPdf;

namespace AnalyticsService.Services.Implementations;

public class ReportEnsService
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
    public async Task<string> PassAvgAnalyse(double? avg , double? pass)
{

    // Créer le prompt pour l'analyse des moyennes des modules et du taux de validation, et les recommandations
    var prompt = $@"
    Ces stats sont d'un enseignant en ecole d'ingénieur:
    - Moyenne des modules : {avg:F1}/5
    - Taux de validation : {pass:F1}%

    Cet enseignant enseigne dans une école d'ingénieur.

    Analyse des performances :
    - **Moyenne des modules (AverageM)** : La moyenne des modules est de {avg:F1}/5. Une moyenne élevée (proche de 5) suggère que les étudiants réussissent bien dans les modules, tandis qu'une faible moyenne pourrait indiquer des difficultés d'enseignement ou des critères d'évaluation trop exigeants.
    - **Taux de validation (PassRate)** : Le taux de validation est de {pass:F1}%, ce qui montre le pourcentage d'étudiants ayant réussi les modules. Un taux élevé montre que la majorité des étudiants réussissent, tandis qu'un taux faible pourrait suggérer que les critères d'évaluation sont trop sévères ou que les étudiants ne sont pas suffisamment préparés.

    **Influence réciproque :**
    - Si **AverageM** est faible, cela pourrait signifier que les étudiants peinent à comprendre certains concepts, ce qui pourrait affecter négativement leur taux de validation (PassRate). Par exemple, si la moyenne est faible, l'enseignant pourrait avoir des difficultés à rendre les modules suffisamment accessibles pour tous les étudiants, entraînant ainsi un faible taux de validation.
    - En revanche, si le **PassRate** est élevé mais que l'**AverageM** est faible, cela pourrait suggérer que l'enseignant ajuste les critères d'évaluation pour faciliter la réussite des étudiants. Par exemple, si l'enseignant choisit de réduire la difficulté des évaluations pour permettre à plus d'étudiants de réussir, cela pourrait faire augmenter le taux de validation tout en maintenant une moyenne plus basse.
    - Si le **PassRate** est faible et la **moyenne** est également faible, cela peut signifier que les critères sont trop difficiles ou mal alignés avec le niveau des étudiants. L'enseignant pourrait avoir besoin de revoir ses méthodes pédagogiques ou d'ajuster ses évaluations pour rendre les modules plus accessibles tout en conservant des critères d'évaluation appropriés.

    Recommandations :
    - Si **AverageM** et **PassRate** sont faibles, il serait recommandé à cet enseignant de :
      1. Revoir les méthodes pédagogiques et ajuster les évaluations.
      2. Offrir plus de ressources ou de soutien aux étudiants, comme des sessions de révision supplémentaires ou des exercices supplémentaires pour améliorer la compréhension.
      3. Reconsidérer la difficulté des critères d'évaluation afin de mieux correspondre au niveau des étudiants.
    - Si **AverageM** est élevé mais **PassRate** est faible, l'enseignant devrait :
      1. Analyser si ses évaluations sont trop difficiles ou si les étudiants ne sont pas suffisamment préparés.
      2. Offrir plus de retours individuels ou des occasions pour les étudiants de récupérer leurs évaluations.
      3. Mettre en place des tests formatifs ou des évaluations intermédiaires pour aider les étudiants à mieux se préparer.
    
    Veuillez fournir une analyse approfondie de l'interdépendance entre ces deux valeurs et des recommandations pour améliorer les performances des étudiants dans les modules. Répond dans une paragraphe sans titre ni sous titres  ";

    // Générer la réponse via GroqAI
    var response = await _groqAi.SendChatAsync(prompt);
    var responseClean = response.Replace("*", "  ");
    return responseClean;
}

    public async Task<byte[]> GenerateEnsReportPassAvgdfAsync(int teacherId)
    {
        try
        {
            // 1) Récupérer les statistiques de l'enseignant
            var stat = await _db.StatistiquesEnseignants.FindAsync(teacherId)
                       ?? throw new KeyNotFoundException("Enseignant introuvable");

            var Response = PassAvgAnalyse(stat.AverageM, stat.PassRate);


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
            var pdfBytes = ReportEnsService.GeneratePdf(htmlContent);
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
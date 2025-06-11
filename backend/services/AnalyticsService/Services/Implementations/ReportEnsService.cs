using AnalyticsService.Data;
using AnalyticsService.ExternalClients.ClientInterfaces;
using AnalyticsService.ExternalClients.OpenAI;
using AnalyticsService.Services.Interfaces;

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
    public async Task<string> PassAvgAnalyse(double avg , double pass)
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
    /*public async Task<byte[]> GenerateEnsReportPdfAsync(int teacherId)
    {
        try
        {
            // 1) Récupérer les statistiques de l'enseignant
            var stat = await _db.StatistiquesEnseignants.FindAsync(teacherId)
                       ?? throw new KeyNotFoundException("Enseignant introuvable");

            // 2) Créer le prompt pour générer le rapport avec GroqAI
            var prompt = $@"Rapport Enseignant {stat.TeacherId} (ISO 29990) :
- Moyenne modules     : {stat.AverageM:F1}/5
- Note Max     : {stat.NoteMax:P0} et Note Min : {stat.NoteMin:P0}
- Feedback positif     : {stat.PositiveFeedBackPct:P0}
- Pourcentage de validation dans les modules enseignés     : {stat.PassRate:F1} h

Selon la norme ISO 29990, indique :
1) Points forts pédagogiques.
2) Axes d'amélioration.
3) Trois recommandations.";

            // 3) Générer le texte du rapport avec GroqAI
            var Response = await _groqAi.SendChatAsync(prompt);

            // Vérifier si le texte généré est valide
            if (string.IsNullOrWhiteSpace(Response))
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
    
    */
}
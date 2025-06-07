using System.Text;
using AutoMapper;
using EvaluationService.DTOs;
using EvaluationService.Models;
using EvaluationService.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EvaluationService.Services;

public class AIAnalyzer : IAIAnalyzer
{
    private readonly IAnswerRepository _answerRepository;
    private readonly IMapper _mapper;
    private readonly HttpClient _client;
    private readonly string? _apiKey;
    private readonly string? _endpoint;
    
    public AIAnalyzer(HttpClient client, IConfiguration cfg, IAnswerRepository answerRepository, IMapper mapper)
    {
        _answerRepository = answerRepository;
        _client = client;
        _mapper = mapper;
        _apiKey = cfg["AI:ApiKey"];
        _endpoint = cfg["AI:Endpoint"];
    } 

    public async Task<List<Answer>> AddCleanAnswersRangeAsync(List<AnswerSubmissionDto> answerSubmissionDtos)
    {
        var answersList = _mapper.Map<List<Answer>>(answerSubmissionDtos);
        var questionsOfAnswers = await _answerRepository.GetQuestionsOfAnswersList(answersList);
        var prompt = new
        {
            instructions = """
                                   Vous êtes un analyseur de réponses. Reçois un JSON:
                                     [ 
                                       answersSubmitted : { QuestionId, RespondentUserId, RawAnswer, RatingAnswer } 
                                     ] et questionsOfAnswers : [ { ...ect } ]
                                   Pour chaque élément:
                                   - Si RatingAnswer est vide et rawAnswer est vide ou illogique par rapport à sa question correspondante dans  
                                   questionsOfAnswers, alors RatingAnswer = -1.
                                   - Sinon, RatingAnswer reçoit une note entre 1 et 5 (double) selon votre analyse du RawAnswer.
                                   et si tu reçoit RatingAnswer non nulle, tu retourne l'objet
                                   { QuestionId, RespondentUserId, RawAnswer, RatingAnswer } tel qu'il est sans analyse,
                                   Tu peut ne pas reçevoir tous les fields mais tu return l'objet comme ci-dessous,
                                   si quelques champs ne sont pas spécifiés tu les mets nulles.
                                   Ne retourne que le tableau JSON sans aucun texte, ni introduction, ni commentaire. Strictement ceci :
                                   [
                                     { "QuestionId": ..., "RespondentUserId": ..., "RawAnswer": ..., "RatingAnswer": ... }
                                   ]
                                   Pas de texte avant ou après. Uniquement le tableau JSON, sans mot autour.
                           """,
            answersSubmitted = answerSubmissionDtos,
            questionsOfAnswers = questionsOfAnswers
        };
        
        var payload = new
        {
            model = "meta-llama/llama-4-scout-17b-16e-instruct",
            temperature = 0.3,
            messages = new[] {
                new
                {
                    role = "user",
                    content = JsonConvert.SerializeObject(prompt)
                }
            }
        };
        
        // Préparer le payload JSON
        
        var req = new HttpRequestMessage(HttpMethod.Post, _endpoint)
        {
            Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json")
        };
        req.Headers.Add("Authorization", $"Bearer {_apiKey}");

        var resp = await _client.SendAsync(req);
        if (!resp.IsSuccessStatusCode)
        {
            var errorText = await resp.Content.ReadAsStringAsync();
            throw new Exception($"AI API Error {resp.StatusCode}: {errorText}");
        }

         var json = await resp.Content.ReadAsStringAsync();
        var doc = JObject.Parse(json);
        var answers = doc["choices"]?[0]?["message"]?["content"]?.ToString();

        if (answers != null)
        {
            var aiAnswersList = JsonConvert.DeserializeObject<List<Answer>>(answers);
            var cleanAnswersList = aiAnswersList?.Where(ca => (float) ca.RatingAnswer != -1).ToList();

            if (cleanAnswersList != null)
            {
                var cleanAddedAnswers = await _answerRepository.AddRangeAsync(cleanAnswersList);
                return cleanAddedAnswers;
            }
        }
        return new List<Answer>();
    }
    
    public async Task<Answer> CleanAnswerUpdateAsync(AnswerSubmissionDto updatedAnswerSubmissionDto)
    {
        var updatedAnswer = _mapper.Map<Answer>(updatedAnswerSubmissionDto);
        var questionOfUpdatedAnswer = await _answerRepository.GetQuestionOfAnswerById(updatedAnswer.AnswerId);
        var payload = new
        {
            instructions = """
                                   Vous êtes un analyseur de réponses. Reçois un JSON:
                                       { QuestionId, RespondentUserId, RawAnswer, RatingAnswer }.
                                   Pour chaque élément:
                                   - Si RatingAnswer est vide et rawAnswer est vide ou illogique par rapport à sa question correspondante dans  
                                   questionOfUpdatedAnswer, alors RatingAnswer = -1.
                                   - Sinon, RatingAnswer reçoit une note entre 1 et 5 (double) selon votre analyse du RawAnswer.
                                   et si tu reçoit RatingAnswer non nulle, tu retourne l'objet
                                   { QuestionId, RespondentUserId, RawAnswer, RatingAnswer } tel qu'il est sans analyse,
                                   Retournez un JSON: 
                                        { QuestionId, RespondentUserId, RawAnswer, RatingAnswer }.
                            """,
            answersSubmitted = updatedAnswerSubmissionDto,
            questionOfUpdatedAnswer = questionOfUpdatedAnswer
        };
        // Préparer le payload JSON

        var req = new HttpRequestMessage(HttpMethod.Post, _endpoint)
        {
            Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json")
        };
        req.Headers.Add("Authorization", $"Bearer {_apiKey}");

        var resp = await _client.SendAsync(req);
        resp.EnsureSuccessStatusCode();

        var json = await resp.Content.ReadAsStringAsync();
        var aiUpdatedAnswer = JsonConvert.DeserializeObject<Answer>(json);
        
        if((float) aiUpdatedAnswer.RatingAnswer != -1)
            await _answerRepository.UpdateAnswerAsync(aiUpdatedAnswer);
        
        return aiUpdatedAnswer;
    }
}

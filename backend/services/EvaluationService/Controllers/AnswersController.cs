using EvaluationService.DTOs;
using EvaluationService.Models;
using EvaluationService.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace EvaluationService.Controllers
{
    [Route("api/answers")]
    [ApiController]
    public class AnswerController : ControllerBase
    {
        private readonly IAnswerService _answerService;

        public AnswerController(IAnswerService answerService)
        {
            _answerService = answerService;
        }
        
        [HttpGet("getAnswersByQuestionId/{questionId:int}")]
        public async Task<IActionResult> GetAnswersByQuestionId(int questionId)
        {
            try
            {
                var answers = await _answerService.GetAnswersByQuestionIdAsync(questionId);
                return Ok();
            }
            catch (Exception e)
            {
                return NotFound("No answers found for the given question ID." + e.Message);
            }
        }

        [HttpGet("getAnswersByQuestionnaireId/{questionnaireId:int}")]
        public async Task<IActionResult> GetAnswersByQuestionnaireId(int questionnaireId)
        {
            try
            {
                var answers = await _answerService.GetAnswersByQuestionnaireIdAsync(questionnaireId);
                if (answers == null || !answers.Any())
                {
                    return NotFound("No answers found for the given questionnaire ID.");
                }
                return Ok(answers);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error fetching answers: " + e.Message);
            }
        }

        [HttpGet("getAnswersByRespondentId/{respondentId}")]
        public async Task<IActionResult> GetAnswersByRespondentId(string respondentId)
        {
            try
            {
                var answers = await _answerService.GetAnswersByRespondentIdAsync(respondentId);
                if (answers == null || !answers.Any())
                {
                    return NotFound("No answers found for the given respondent ID.");
                }

                return Ok(answers);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error fetching answers: " + e.Message);
            }
        }

        [HttpGet("getAllAnswers")]
        public async Task<IActionResult> GetAllAnswers()
        {
            try
            {
                var answers = await _answerService.GetAllAnswersAsync();
                return Ok(answers);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error fetching answers: " + e.Message);
            }
        }

        [HttpGet("getAnswerById/{answerId:int}")]
        public async Task<IActionResult> GetAnswerById(int answerId)
        {
            try{
                var answer = await _answerService.GetAnswerByIdAsync(answerId);
                return Ok(answer);
            }
            catch (Exception e)
            {
                return NotFound("No answer found for the given answer ID." + e.Message);
            }
        }

        [HttpPost("addCleanAnswer")]
        public async Task<IActionResult> AddCleanAnswer([FromBody] AnswerSubmissionDto answerSubmissionDto)
        {
            if (answerSubmissionDto == null)
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                var newAnswer = await _answerService.AddCleanAnswerAsync(answerSubmissionDto);
                return CreatedAtAction(nameof(GetAnswerById), new { answerId = newAnswer.AnswerId }, newAnswer);
            }
            catch (Exception e)
            {
                return StatusCode(500, "An error occurred while adding answers: " + e.Message);
            }
        }

        [HttpPut("updateCleanAnswer")]
        public async Task<IActionResult> UpdateCleanAnswer([FromBody] Answer updatedAnswer)
        {
            if (updatedAnswer == null)
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                var answer = await _answerService.UpdateCleanAnswerAsync(updatedAnswer);
                return Ok(answer);
            }
            catch (Exception e)
            {
                return NotFound("No answer found for the given answer ID." + e.Message);
            }
        }

        [HttpDelete("deleteAnswerById/{answerId:int}")]
        public async Task<IActionResult> DeleteAnswerById(int answerId)
        {
            try{
                var answer = await _answerService.DeleteAnswerByIdAsync(answerId);
                return Ok(answer);
            }
            catch (Exception e)
            {
                return NotFound("No answer found for the given answer ID." + e.Message);
            }
        }
        
        [HttpPost("addCleanRangeOfAnswers")]
        public async Task<IActionResult> AddCleanRangeOfAnswers([FromBody] List<AnswerSubmissionDto> dtoAnswerSubmissions)
        {
            if (dtoAnswerSubmissions == null || !dtoAnswerSubmissions.Any())
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                var answers = await _answerService.AddCleanRangeAsync(dtoAnswerSubmissions);
                return Ok(answers);
            }
            catch (Exception e)
            {
                return StatusCode(500, "An error occurred while adding answers: " + e.Message);
            }
        }

        [HttpGet("getAnswerTypeFiliereModule/{answerId:int}")]
        public async Task<IActionResult> GetAnswerTypeFiliereModule(int answerId)
        {
            try
            {
                var answerType = await _answerService.GetAnswerTypeFiliereModuleAsync(answerId);
                if (answerType == "Invalid_Questionnaire_Type")
                {
                    return NotFound("Invalid Answer type for the given answer ID.");
                }

                return Ok(answerType);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error Occured" + e.Message);
            }
        }

        [HttpGet("getQuestionnaireTypeInternalExternal/{answerId:int}")]
        public async Task<IActionResult> GetQuestionnaireTypeInternalExternal(int answerId)
        {
            try{
                var questionnaireType = await _answerService.GetQuestionnaireTypeInternalExternalAsync(answerId);
                return Ok(questionnaireType);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error Occured Or Invalid Questionnaire type for the given answer ID." + e.Message);
            }
        }

        [HttpGet("getAnswersFiliere")]
        public async Task<IActionResult> GetAnswersFiliere()
        {
            try
            {
                var answers = await _answerService.GetAnswersFiliereAsync();
                if (answers == null || !answers.Any())
                {
                    return NotFound("No answers found for the given questionnaire ID.");
                }
                return Ok(answers);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error fetching answers: " + e.Message);
            }
        }

        [HttpGet("getAnswersModule")]
        public async Task<IActionResult> GetAnswersModule()
        {
            try
            {
                var answers = await _answerService.GetAnswersModuleAsync();
                if (answers == null || !answers.Any())
                {
                    return NotFound("No answers found for the given questionnaire ID.");
                }

                return Ok(answers);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error fetching answers: " + e.Message);
            }
        }

        [HttpPost("questions-of-answers")]
        public async Task<IActionResult> GetQuestionsOfAnswersList([FromBody] List<Answer> answers)
        {
            try
            {
                var questionsOfAnswers = await _answerService.GetQuestionsOfAnswersList(answers);
                return Ok(questionsOfAnswers);
            }
            catch (Exception ex)
            {
                return BadRequest("Error occured while fetching questions of answers: " + ex.Message);
            }
        }

        [HttpGet("question-of-answer-by-id/{answerId:int}")]
        public async Task<IActionResult> GetQuestionOfAnswerById(int answerId)
        {
            try
            {
                var questionOfAnswer = await _answerService.GetQuestionOfAnswerById(answerId);
                return Ok(questionOfAnswer);
            }
            catch (Exception ex)
            {
                return BadRequest("Error occured while fetching question of answer: " + ex.Message);
            }
        }      
        
        [HttpPost("question-of-answer")]
        public async Task<IActionResult> GetQuestionOfAnswer([FromBody] Answer answer)
        {
            try
            {
                var questionOfAnswer = await _answerService.GetQuestionOfAnswer(answer);
                return Ok(questionOfAnswer);
            }
            catch (Exception ex)
            {
                return BadRequest("Error occured while fetching question of answer: " + ex.Message);
            }
        }
    }
}


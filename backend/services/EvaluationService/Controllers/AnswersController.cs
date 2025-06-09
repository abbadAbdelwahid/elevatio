using EvaluationService.DTOs;
using EvaluationService.Models;
using EvaluationService.Services;
using Microsoft.AspNetCore.Mvc;

namespace EvaluationService.Controllers
{
    [ApiController]
    [Route("api/answers")]
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
                return Ok(answers);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error fetching answers by question ID: " + e.Message);
            }
        }

        [HttpGet("getAnswersByQuestionnaireId/{questionnaireId:int}")]
        public async Task<IActionResult> GetAnswersByQuestionnaireId(int questionnaireId)
        {
            try
            {
                var answers = await _answerService.GetAnswersByQuestionnaireIdAsync(questionnaireId);
                return Ok(answers);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error fetching answers by questionnaire ID: " + e.Message);
            }
        }

        [HttpGet("getAnswersByRespondentId/{respondentId}")]
        public async Task<IActionResult> GetAnswersByRespondentId(string respondentId)
        {
            try
            {
                var answers = await _answerService.GetAnswersByRespondentIdAsync(respondentId);
                return Ok(answers);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error fetching answers by respondent ID: " + e.Message);
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
                return StatusCode(500, "Error fetching all answers: " + e.Message);
            }
        }

        [HttpGet("getAnswerById/{answerId:int}")]
        public async Task<IActionResult> GetAnswerById(int answerId)
        {
            try
            {
                var answer = await _answerService.GetAnswerByIdAsync(answerId);
                return Ok(answer);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound("Error fetching answer by ID: " + ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error fetching answer by ID: " + e.Message);
            }
        }

        [HttpPost("addCleanAnswer")]
        public async Task<IActionResult> AddCleanAnswer([FromBody] AnswerSubmissionDto answerSubmissionDto)
        {
            if (answerSubmissionDto == null)
                return BadRequest("Invalid data.");

            try
            {
                var newAnswer = await _answerService.AddCleanAnswerAsync(answerSubmissionDto);
                return CreatedAtAction(nameof(GetAnswerById),
                                       new { answerId = newAnswer.AnswerId },
                                       newAnswer);
            }
            catch (Exception e)
            {
                return StatusCode(500, "An error occurred while adding the answer: " + e.Message);
            }
        }

        [HttpPut("updateCleanAnswer")]
        public async Task<IActionResult> UpdateCleanAnswer([FromBody] Answer updatedAnswer)
        {
            if (updatedAnswer == null)
                return BadRequest("Invalid data.");

            try
            {
                var answer = await _answerService.UpdateCleanAnswerAsync(updatedAnswer);
                return Ok(answer);
            }   
            catch (KeyNotFoundException ex)
            {
                return NotFound("Error updating answer: " + ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error updating answer: " + e.Message);
            }
        }

        [HttpDelete("deleteAnswerById/{answerId:int}")]
        public async Task<IActionResult> DeleteAnswerById(int answerId)
        {
            try
            {
                var answer = await _answerService.DeleteAnswerByIdAsync(answerId);
                return Ok(answer);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound("Error deleting answer: " + ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error deleting answer: " + e.Message);
            }
        }
        
        [HttpDelete("deleteAnswerByRespondentId/{respondentId}")]
        public async Task<IActionResult> DeleteAnswersByRespondentId(string respondentId)
        {
            try
            {
                var answer = await _answerService.DeleteAnswersByRespondentId(respondentId);
                return Ok(answer);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error deleting answer: " + e.Message);
            }
        }

        [HttpPost("addCleanRangeOfAnswers")]
        public async Task<IActionResult> AddCleanRangeOfAnswers([FromBody] List<AnswerSubmissionDto> dtoAnswerSubmissions)
        {
            if (dtoAnswerSubmissions == null || !dtoAnswerSubmissions.Any())
                return BadRequest("Invalid data.");

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
                var type = await _answerService.GetQuestionnaireTypeFiliereModuleAsync(answerId);
                return Ok(new { TypeModuleFiliere = type});
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound("Error fetching answer: " + ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error fetching answer type filiere/module: " + e.Message);
            }
        }

        [HttpGet("getQuestionnaireTypeInternalExternal/{answerId:int}")]
        public async Task<IActionResult> GetQuestionnaireTypeInternalExternal(int answerId)
        {
            try
            {
                var type = await _answerService.GetQuestionnaireTypeInternalExternalAsync(answerId);
                return Ok(new { TypeInternalExternal = type});
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound("Error fetching answer: " + ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error fetching questionnaire type internal/external: " + e.Message);
            }
        }

        [HttpGet("getAnswersFiliere")]
        public async Task<IActionResult> GetAnswersFiliere()
        {
            try
            {
                var answers = await _answerService.GetAnswersFiliereAsync();
                return Ok(answers);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error fetching filiere answers: " + e.Message);
            }
        }

        [HttpGet("getAnswersModule")]
        public async Task<IActionResult> GetAnswersModule()
        {
            try
            {
                var answers = await _answerService.GetAnswersModuleAsync();
                return Ok(answers);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error fetching module answers: " + e.Message);
            }
        }

        [HttpPost("questions-of-answers")]
        public async Task<IActionResult> GetQuestionsOfAnswersList([FromBody] List<Answer> answers)
        {
            if (answers == null || !answers.Any())
                return BadRequest("Invalid answers list.");

            try
            {
                var questions = await _answerService.GetQuestionsOfAnswersList(answers);
                return Ok(questions);
            }
            catch (Exception e)
            {
                return BadRequest("Error fetching questions of answers: " + e.Message);
            }
        }

        [HttpGet("question-of-answer-by-id/{answerId:int}")]
        public async Task<IActionResult> GetQuestionOfAnswerById(int answerId)
        {
            try
            {
                var question = await _answerService.GetQuestionOfAnswerById(answerId);
                return Ok(question);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound("Error fetching answer: " + ex.Message);
            }
            catch (Exception e)
            {
                return BadRequest("Error fetching question of answer: " + e.Message);
            }
        }
    }
}

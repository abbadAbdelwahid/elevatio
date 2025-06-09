using Microsoft.AspNetCore.Mvc;

using EvaluationService.DTOs;
using EvaluationService.Models;
using EvaluationService.Services;

namespace EvaluationService.Controllers
{
    [Route("api/questions")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpGet("GetAllQuestions")]
        public async Task<IActionResult> GetAllQuestions()
        {
            try
            {
                var questions = await _questionService.GetAllQuestionsAsync();
                return Ok(questions);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error fetching questions: " + e.Message);
            }
        }
        
        [HttpPost("addRange")]
        public async Task<IActionResult> AddRange([FromBody] List<CreateQuestionDto> questionsDto)
        {
            if (questionsDto == null || !questionsDto.Any())
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                var questions = await _questionService.AddRangeAsync(questionsDto);
                return Ok(questions);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error adding questions: " + e.Message);
            }
        }

        [HttpGet("getQuestionsByQuestionnaireId/{questionnaireId:int}")]
        public async Task<IActionResult> GetQuestionsByQuestionnaireId(int questionnaireId)
        {
            try
            {
                var questions = await _questionService.GetQuestionsByQuestionnaireIdAsync(questionnaireId);
                return Ok(questions);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error fetching questions by questionnaire ID: " + e.Message);
            }
        }

        [HttpGet("getStandardQuestions/{questionnaireId:int}")]
        public async Task<IActionResult> GetStandardQuestions(int questionnaireId)
        {
            try
            {
                var standardQuestions = await _questionService.GetStandardQuestionsAsync(questionnaireId);
                return Ok(standardQuestions);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error fetching standard questions: " + e.Message);
            }
        }

        [HttpPost("addQuestion")]
        public async Task<IActionResult> AddQuestion([FromBody] CreateQuestionDto questionDto)
        {
            if (questionDto == null)
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                var newQuestion = await _questionService.AddQuestionAsync(questionDto);
                return CreatedAtAction(nameof(GetQuestion), new { questionId = newQuestion.QuestionId }, newQuestion);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error adding question: " + e.Message);
            }
        }

        [HttpGet("getQuestion/{questionId:int}")]
        public async Task<IActionResult> GetQuestion(int questionId)
        {
            try
            {
                var question = await _questionService.GetQuestionAsync(questionId);
                return Ok(question);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound("Error fetching question: " + ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error fetching question: " + e.Message);
            }
        }

        [HttpDelete("deleteQuestion/{questionId:int}")]
        public async Task<IActionResult> DeleteQuestion(int questionId)
        {
            try
            {
                var deletedQuestion = await _questionService.DeleteQuestionAsync(questionId);
                return Ok(deletedQuestion);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound("Error fetching question: " + ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error deleting question: " + e.Message);
            }
        }

        [HttpPut("updateQuestion")]
        public async Task<IActionResult> UpdateQuestion([FromBody] Question questionDto)
        {
            if (questionDto == null)
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                var updatedQuestion = await _questionService.UpdateQuestionAsync(questionDto);
                return Ok(updatedQuestion);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound("Error fetching question: " + ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error updating question: " + e.Message);
            }
        }
    }
}

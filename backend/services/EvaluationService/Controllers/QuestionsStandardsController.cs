using Microsoft.AspNetCore.Mvc;

using EvaluationService.DTOs;
using EvaluationService.Models;
using EvaluationService.Services;

namespace EvaluationService.Controllers
{
    [Route("api/standardQuestions")]
    [ApiController]
    public class StandardQuestionController : ControllerBase
    {
        private readonly IQuestionsStandardService _standardQuestionService;

        public StandardQuestionController(IQuestionsStandardService standardQuestionService)
        {
            _standardQuestionService = standardQuestionService;
        }

        [HttpGet("getQuestionsByStatName")]
        public async Task<IActionResult> GetQuestionsStandardsByStatName([FromQuery] StatName statName)
        {
            try
            {
                var questions = await _standardQuestionService.GetQuestionsStandardsByStatName(statName);
                return Ok(questions);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error fetching standard questions: " + e.Message);
            }
        }

        [HttpGet("getStandardQuestionById/{id:int}")]
        public async Task<IActionResult> GetStandardQuestionById(int id)
        {
            try
            {
                var question = await _standardQuestionService.GetStandardQuestionById(id);
                return Ok(question);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound("Error fetching question standard: " + ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error fetching standard question: " + e.Message);
            }
        }

        [HttpGet("getStandardQuestions")]
        public async Task<IActionResult> GetStandardQuestions()
        {
            try
            {
                var questions = await _standardQuestionService.GetStandardQuestions();
                return Ok(questions);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error fetching standard questions: " + e.Message);
            }
        }

        [HttpDelete("deleteStandardQuestionById/{id:int}")]
        public async Task<IActionResult> DeleteStandardQuestionById(int id)
        {
            try
            {
                var deletedQuestion = await _standardQuestionService.DeleteStandardQuestionById(id);
                return Ok(deletedQuestion);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound("Error fetching question standard: " + ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error deleting standard question: " + e.Message);
            }
        }

        [HttpDelete("deleteStandardQuestionsByStatName")]
        public async Task<IActionResult> DeleteStandardQuestionsByStatName([FromQuery] StatName statName)
        {
            try
            {
                var deletedQuestions = await _standardQuestionService.DeleteStandardQuestionsByStatName(statName);
                return Ok(deletedQuestions);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error deleting standard questions: " + e.Message);
            }
        }

        [HttpPost("addStandardQuestion")]
        public async Task<IActionResult> AddStandardQuestion([FromBody] CreateStandardQuestionDto createStandardQuestionDto)
        {
            if (createStandardQuestionDto == null)
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                var newQuestion = await _standardQuestionService.AddStandardQuestion(createStandardQuestionDto);
                return CreatedAtAction(nameof(GetStandardQuestionById), new { id = newQuestion.StandardQuestionId }, newQuestion);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error adding standard question: " + e.Message);
            }
        }

        [HttpPut("updateStandardQuestion")]
        public async Task<IActionResult> UpdateStandardQuestion([FromBody] StandardQuestion updatedStandardQuestion)
        {
            if (updatedStandardQuestion == null)
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                var updatedQuestion = await _standardQuestionService.UpdateStandardQuestion(updatedStandardQuestion);
                return Ok(updatedQuestion);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound("Error fetching question standard: " + ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error updating standard question: " + e.Message);
            }
        }
    }
}

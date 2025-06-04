using EvaluationService.DTOs;
using EvaluationService.Models;
using EvaluationService.Services;
using Microsoft.AspNetCore.Mvc;

namespace EvaluationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionnaireController : ControllerBase
    {
        private readonly IQuestionnaireService _questionnaireService;

        public QuestionnaireController(IQuestionnaireService questionnaireService)
        {
            _questionnaireService = questionnaireService;
        }

        [HttpPost("addStandardQuestionsToQuestionnaire/{questionnaireId:int}")]
        public async Task<IActionResult> AddStandardQuestionsToQuestionnaire(int questionnaireId, [FromQuery] StatName statName)
        {
            try
            {
                var questions = await _questionnaireService.AddStandardQuestionsToQuestionnaireAsync(questionnaireId, statName);
                return Ok(questions);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error adding standard questions: " + e.Message);
            }
        }

        [HttpPost("addQuestionsToQuestionnaire/{questionnaireId:int}")]
        public async Task<IActionResult> AddQuestionsToQuestionnaire(int questionnaireId, [FromBody] List<Question> questions)
        {
            if (questions == null || !questions.Any())
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                var addedQuestions = await _questionnaireService.AddQuestionsToQuestionnaireAsync(questionnaireId, questions);
                return Ok(addedQuestions);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error adding questions: " + e.Message);
            }
        }

        [HttpGet("getStandardQuestions/{questionnaireId:int}")]
        public async Task<IActionResult> GetStandardQuestions(int questionnaireId)
        {
            try
            {
                var questions = await _questionnaireService.GetStandardQuestionsAsync(questionnaireId);
                return Ok(questions);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error fetching standard questions: " + e.Message);
            }
        }

        [HttpGet("getQuestionnaireTypeExternalInternal/{questionnaireId:int}")]
        public async Task<IActionResult> GetQuestionnaireTypeExternalInternal(int questionnaireId)
        {
            try
            {
                var questionnaireType = await _questionnaireService.GetQuestionnaireTypeExternalInternalAsync(questionnaireId);
                return Ok(questionnaireType);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error fetching questionnaire type: " + e.Message);
            }
        }

        [HttpGet("getQuestionnairesTypeExternal")]
        public async Task<IActionResult> GetQuestionnairesTypeExternal()
        {
            try
            {
                var questionnaires = await _questionnaireService.GetQuestionnairesTypeExternalAsync();
                return Ok(questionnaires);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error fetching external questionnaires: " + e.Message);
            }
        }

        [HttpGet("getQuestionnairesTypeInternal")]
        public async Task<IActionResult> GetQuestionnairesTypeInternal()
        {
            try
            {
                var questionnaires = await _questionnaireService.GetQuestionnairesTypeInternalAsync();
                return Ok(questionnaires);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error fetching internal questionnaires: " + e.Message);
            }
        }

        [HttpGet("getQuestionnaireTypeModuleFiliere/{questionnaireId:int}")]
        public async Task<IActionResult> GetQuestionnaireTypeModuleFiliere(int questionnaireId)
        {
            try
            {
                var questionnaireType = await _questionnaireService.GetQuestionnaireTypeModuleFiliereAsync(questionnaireId);
                return Ok(questionnaireType);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error fetching questionnaire type for module or filière: " + e.Message);
            }
        }

        [HttpGet("getQuestionnaireById/{questionnaireId:int}")]
        public async Task<IActionResult> GetQuestionnaireById(int questionnaireId)
        {
            try
            {
                var questionnaire = await _questionnaireService.GetQuestionnaireByIdAsync(questionnaireId);
                if (questionnaire == null)
                {
                    return NotFound("Questionnaire not found for the given ID.");
                }
                return Ok(questionnaire);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error fetching questionnaire: " + e.Message);
            }
        }

        [HttpGet("getQuestionnairesByModuleId/{moduleId:int}")]
        public async Task<IActionResult> GetQuestionnairesByModuleId(int moduleId)
        {
            try
            {
                var questionnaires = await _questionnaireService.GetQuestionnairesByModuleIdAsync(moduleId);
                if (questionnaires == null || !questionnaires.Any())
                {
                    return NotFound("No questionnaires found for the given module ID.");
                }
                return Ok(questionnaires);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error fetching questionnaires by module ID: " + e.Message);
            }
        }

        [HttpGet("getQuestionnairesByFiliereId/{filiereId:int}")]
        public async Task<IActionResult> GetQuestionnairesByFiliereId(int filiereId)
        {
            try
            {
                var questionnaires = await _questionnaireService.GetQuestionnairesByFiliereIdAsync(filiereId);
                if (questionnaires == null || !questionnaires.Any())
                {
                    return NotFound("No questionnaires found for the given filière ID.");
                }
                return Ok(questionnaires);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error fetching questionnaires by filière ID: " + e.Message);
            }
        }

        [HttpGet("getQuestionnairesByCreatorUserId/{creatorUserId}")]
        public async Task<IActionResult> GetQuestionnairesByCreatorUserId(string creatorUserId)
        {
            try
            {
                var questionnaires = await _questionnaireService.GetQuestionnairesByCreatorUserIdAsync(creatorUserId);
                if (questionnaires == null || !questionnaires.Any())
                {
                    return NotFound("No questionnaires found for the given creator user ID.");
                }
                return Ok(questionnaires);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error fetching questionnaires by creator user ID: " + e.Message);
            }
        }

        [HttpDelete("deleteQuestionnaire/{questionnaireId:int}")]
        public async Task<IActionResult> DeleteQuestionnaire(int questionnaireId)
        {
            try
            {
                var questionnaire = await _questionnaireService.DeleteQuestionnaireAsync(questionnaireId);
                if (questionnaire == null)
                {
                    return NotFound("Questionnaire not found for the given ID.");
                }
                return Ok(questionnaire);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error deleting questionnaire: " + e.Message);
            }
        }
        
        [HttpDelete("deleteQuestionnaireByRespondentId/{respondentId}")]
        public async Task<IActionResult> DeleteQuestionnairesByRespondentId(string respondentId)
        {
            try
            {
                var questionnaire = await _questionnaireService.DeleteQuestionnairesByRespondentIdAsync(respondentId);
                if (questionnaire == null)
                    return NotFound("Questionnaire not found for the given respondent ID.");
                return Ok(questionnaire);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error deleting questionnaire: " + e.Message);
            }
        }
        
        [HttpDelete("deleteQuestionnaireByCreatorId/{creatorId}")]
        public async Task<IActionResult> DeleteQuestionnairesByCreatorId(string creatorId)
        {
            try
            {
                var questionnaire = await _questionnaireService.DeleteQuestionnairesByCreatorIdAsync(creatorId);
                if (questionnaire == null)
                    return NotFound("Questionnaire not found for the given creator ID.");
                return Ok(questionnaire);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error deleting questionnaire: " + e.Message);
            }
        }
        
        [HttpDelete("deleteQuestionnaireByFiliereId/{filiereId:int}")]
        public async Task<IActionResult> DeleteQuestionnairesByFiliereId(int filiereId)
        {
            try
            {
                var questionnaire = await _questionnaireService.DeleteQuestionnairesByFiliereIdAsync(filiereId);
                if (questionnaire == null)
                    return NotFound("Questionnaire not found for the given filiere ID.");
                return Ok(questionnaire);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error deleting questionnaire: " + e.Message);
            }
        }

        [HttpDelete("deleteQuestionnairesByModuleId/{moduleId:int}")]
        public async Task<IActionResult> DeleteQuestionnairesByModuleId(int moduleId)
        {
            try
            {
                var questionnaire = await _questionnaireService.DeleteQuestionnairesByModuleIdAsync(moduleId);
                if (questionnaire == null)
                    return NotFound("Questionnaire not found for the given module ID.");
                return Ok(questionnaire);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error deleting questionnaire: " + e.Message);
            }
        }

        [HttpPut("updateQuestionnaire")]
        public async Task<IActionResult> UpdateQuestionnaire([FromBody] Questionnaire questionnaire)
        {
            if (questionnaire == null)
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                var updatedQuestionnaire = await _questionnaireService.UpdateQuestionnaireAsync(questionnaire);
                return Ok(updatedQuestionnaire);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error updating questionnaire: " + e.Message);
            }
        }

        [HttpPost("addQuestionnaire")]
        public async Task<IActionResult> AddQuestionnaire([FromBody] CreateQuestionnaireDto createQuestionnaireDto)
        {
            if (createQuestionnaireDto == null)
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                var newQuestionnaire = await _questionnaireService.AddQuestionnaireAsync(createQuestionnaireDto);
                return CreatedAtAction(nameof(GetQuestionnaireById), new { questionnaireId = newQuestionnaire.QuestionnaireId }, newQuestionnaire);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error adding questionnaire: " + e.Message);
            }
        }

        [HttpGet("getAllQuestionnaires")]
        public async Task<IActionResult> GetAllQuestionnaires()
        {
            try
            {
                var questionnaires = await _questionnaireService.GetAllQuestionnairesAsync();
                return Ok(questionnaires);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error fetching all questionnaires: " + e.Message);
            }
        }

        [HttpGet("getAllQuestionnairesFiliere")]
        public async Task<IActionResult> GetAllQuestionnairesFiliere()
        {
            try
            {
                var questionnaires = await _questionnaireService.GetAllQuestionnairesFiliereAsync();
                return Ok(questionnaires);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error fetching questionnaires by filière: " + e.Message);
            }
        }

        [HttpGet("getAllQuestionnairesModule")]
        public async Task<IActionResult> GetAllQuestionnairesModule()
        {
            try
            {
                var questionnaires = await _questionnaireService.GetAllQuestionnairesModuleAsync();
                return Ok(questionnaires);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error fetching questionnaires by module: " + e.Message);
            }
        }

        [HttpGet("getAllQuestionnairesByCreatorUserId/{creatorUserId}")]
        public async Task<IActionResult> GetAllQuestionnairesByCreatorUserId(string creatorUserId)
        {
            try
            {
                var questionnaires = await _questionnaireService.GetAllQuestionnairesByCreatorUserIdAsync(creatorUserId);
                return Ok(questionnaires);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error fetching questionnaires by creator user ID: " + e.Message);
            }
        }

        [HttpGet("getRespondentIdByQuestionnaireId/{questionnaireId:int}")]
        public async Task<IActionResult> GetRespondentIdByQuestionnaireId(int questionnaireId)
        {
            try
            {
                var respondentId = await _questionnaireService.GetRespondentIdByQuestionnaireIdAsync(questionnaireId);
                return Ok(respondentId);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error fetching respondent ID: " + e.Message);
            }
        }

        [HttpGet("getQuestionnairesByRespondentId/{respondentId}")]
        public async Task<IActionResult> GetQuestionnairesByRespondentId(string respondentId)
        {
            try
            {
                var questionnaires = await _questionnaireService.GetQuestionnairesByRespondentIdAsync(respondentId);
                return Ok(questionnaires);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error fetching questionnaires by respondent ID: " + e.Message);
            }
        }
    }
}

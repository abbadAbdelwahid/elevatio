using EvaluationService.DTOs;
using EvaluationService.Models;
using EvaluationService.Services;
using Microsoft.AspNetCore.Mvc;

namespace EvaluationService.Controllers
{
    [ApiController]
    [Route("api/questionnaires")]
    public class QuestionnaireController : ControllerBase
    {
        private readonly IQuestionnaireService _questionnaireService;

        public QuestionnaireController(IQuestionnaireService questionnaireService)
        {
            _questionnaireService = questionnaireService;
        }

        [HttpPost("addStandardQuestions/{questionnaireId:int}")]
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

        [HttpPost("addQuestions/{questionnaireId:int}")]
        public async Task<IActionResult> AddQuestionsToQuestionnaire(int questionnaireId, [FromBody] List<CreateQuestionWithQuestionnaire> questionsDto)
        {
            if (questionsDto == null || !questionsDto.Any())
                return BadRequest("Invalid data.");

            try
            {
                var added = await _questionnaireService.AddQuestionsToQuestionnaireAsync(questionnaireId, questionsDto);
                return Ok(added);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error adding questions: " + e.Message);
            }
        }

        [HttpGet("standardQuestions/{questionnaireId:int}")]
        public async Task<IActionResult> GetStandardQuestions(int questionnaireId)
        {
            try
            {
                var list = await _questionnaireService.GetStandardQuestionsAsync(questionnaireId);
                return Ok(list);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error fetching standard questions: " + e.Message);
            }
        }

        [HttpGet("type/external-internal/{questionnaireId:int}")]
        public async Task<IActionResult> GetQuestionnaireTypeExternalInternal(int questionnaireId)
        {
            try
            {
                var type = await _questionnaireService.GetQuestionnaireTypeExternalInternalAsync(questionnaireId);
                if (type == null)
                    return NotFound($"No external/internal type for questionnaire {questionnaireId}.");
                return Ok(new {  typeInternalExternal = type });
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error fetching questionnaire type: " + e.Message);
            }
        }

        [HttpGet("type/module-filiere/{questionnaireId:int}")]
        public async Task<IActionResult> GetQuestionnaireTypeModuleFiliere(int questionnaireId)
        {
            try
            {
                var type = await _questionnaireService.GetQuestionnaireTypeModuleFiliereAsync(questionnaireId);
                if (type == null)
                    return NotFound($"No module/filière type for questionnaire {questionnaireId}.");
                return Ok(new {  typeModuleFiliere = type });
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error fetching questionnaire type: " + e.Message);
            }
        }

        [HttpGet("type/external")]
        public async Task<IActionResult> GetQuestionnairesTypeExternal()
        {
            try
            {
                var list = await _questionnaireService.GetQuestionnairesTypeExternalAsync();
                return Ok(list);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error fetching external questionnaires: " + e.Message);
            }
        }

        [HttpGet("type/internal")]
        public async Task<IActionResult> GetQuestionnairesTypeInternal()
        {
            try
            {
                var list = await _questionnaireService.GetQuestionnairesTypeInternalAsync();
                return Ok(list);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error fetching internal questionnaires: " + e.Message);
            }
        }

        [HttpGet("{questionnaireId:int}")]
        public async Task<IActionResult> GetQuestionnaireById(int questionnaireId)
        {
            try
            {
                var q = await _questionnaireService.GetQuestionnaireByIdAsync(questionnaireId);
                if (q == null)
                    return NotFound($"Questionnaire {questionnaireId} not found.");
                return Ok(q);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error fetching questionnaire: " + e.Message);
            }
        }

        [HttpGet("by-module/{moduleId:int}")]
        public async Task<IActionResult> GetByModule(int moduleId)
        {
            try
            {
                var list = await _questionnaireService.GetQuestionnairesByModuleIdAsync(moduleId);
                if (list == null || !list.Any())
                    return NotFound($"No questionnaires for module {moduleId}.");
                return Ok(list);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error fetching by module: " + e.Message);
            }
        }

        [HttpGet("by-filiere/{filiereId:int}")]
        public async Task<IActionResult> GetByFiliere(int filiereId)
        {
            try
            {
                var list = await _questionnaireService.GetQuestionnairesByFiliereIdAsync(filiereId);
                if (list == null || !list.Any())
                    return NotFound($"No questionnaires for filière {filiereId}.");
                return Ok(list);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error fetching by filière: " + e.Message);
            }
        }

        [HttpGet("by-creator/{creatorUserId}")]
        public async Task<IActionResult> GetByCreator(string creatorUserId)
        {
            try
            {
                var list = await _questionnaireService.GetQuestionnairesByCreatorUserIdAsync(creatorUserId);
                if (list == null || !list.Any())
                    return NotFound($"No questionnaires for creator {creatorUserId}.");
                return Ok(list);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error fetching by creator: " + e.Message);
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = await _questionnaireService.GetAllQuestionnairesAsync();
                return Ok(list);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error fetching all questionnaires: " + e.Message);
            }
        }

        [HttpGet("all/filiere")]
        public async Task<IActionResult> GetAllFiliere()
        {
            try
            {
                var list = await _questionnaireService.GetAllQuestionnairesFiliereAsync();
                return Ok(list);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error fetching filière questionnaires: " + e.Message);
            }
        }

        [HttpGet("all/module")]
        public async Task<IActionResult> GetAllModule()
        {
            try
            {
                var list = await _questionnaireService.GetAllQuestionnairesModuleAsync();
                return Ok(list);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error fetching module questionnaires: " + e.Message);
            }
        }

        [HttpGet("all/by-creator/{creatorUserId}")]
        public async Task<IActionResult> GetAllByCreator(string creatorUserId)
        {
            try
            {
                var list = await _questionnaireService.GetAllQuestionnairesByCreatorUserIdAsync(creatorUserId);
                return Ok(list);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error fetching by creator: " + e.Message);
            }
        }

        [HttpGet("respondents/{questionnaireId:int}")]
        public async Task<IActionResult> GetRespondents(int questionnaireId)
        {
            try
            {
                var ids = await _questionnaireService.GetRespondentsIdsByQuestionnaireIdAsync(questionnaireId);
                if (ids == null || !ids.Any())
                    return NotFound($"No respondents for questionnaire {questionnaireId}.");
                return Ok(ids);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error fetching respondents: " + e.Message);
            }
        }

        [HttpGet("by-respondent/{respondentId}")]
        public async Task<IActionResult> GetByRespondent(string respondentId)
        {
            try
            {
                var list = await _questionnaireService.GetQuestionnairesByRespondentIdAsync(respondentId);
                if (list == null || !list.Any())
                    return NotFound($"No questionnaires for respondent {respondentId}.");
                return Ok(list);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error fetching by respondent: " + e.Message);
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddQuestionnaire([FromBody] CreateQuestionnaireDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid data.");

            try
            {
                var created = await _questionnaireService.AddQuestionnaireAsync(dto);
                return CreatedAtAction(nameof(GetQuestionnaireById), new { questionnaireId = created.QuestionnaireId }, created);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error adding questionnaire: " + e.Message);
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateQuestionnaire([FromBody] Questionnaire q)
        {
            if (q == null)
                return BadRequest("Invalid data.");

            try
            {
                var updated = await _questionnaireService.UpdateQuestionnaireAsync(q);
                return Ok(updated);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error updating questionnaire: " + e.Message);
            }
        }

        [HttpDelete("{questionnaireId:int}")]
        public async Task<IActionResult> DeleteQuestionnaire(int questionnaireId)
        {
            try
            {
                var deleted = await _questionnaireService.DeleteQuestionnaireAsync(questionnaireId);
                if (deleted == null)
                    return NotFound($"Questionnaire {questionnaireId} not found.");
                return Ok(deleted);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error deleting questionnaire: " + e.Message);
            }
        }

        [HttpDelete("by-creator/{creatorId}")]
        public async Task<IActionResult> DeleteByCreator(string creatorId)
        {
            try
            {
                var list = await _questionnaireService.DeleteQuestionnairesByCreatorIdAsync(creatorId);
                if (list == null || !list.Any())
                    return NotFound($"No questionnaires for creator {creatorId}.");
                return Ok(list);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error deleting by creator: " + e.Message);
            }
        }

        [HttpDelete("by-filiere/{filiereId:int}")]
        public async Task<IActionResult> DeleteByFiliere(int filiereId)
        {
            try
            {
                var list = await _questionnaireService.DeleteQuestionnairesByFiliereIdAsync(filiereId);
                if (list == null || !list.Any())
                    return NotFound($"No questionnaires for filière {filiereId}.");
                return Ok(list);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error deleting by filière: " + e.Message);
            }
        }

        [HttpDelete("by-module/{moduleId:int}")]
        public async Task<IActionResult> DeleteByModule(int moduleId)
        {
            try
            {
                var list = await _questionnaireService.DeleteQuestionnairesByModuleIdAsync(moduleId);
                if (list == null || !list.Any())
                    return NotFound($"No questionnaires for module {moduleId}.");
                return Ok(list);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error deleting by module: " + e.Message);
            }
        }

        [HttpPost("cascade-delete")]
        public async Task<IActionResult> HandlingCascadeDeletion()
        {
            try
            {
                await _questionnaireService.HandlingCascadeDeletion();
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error during cascade deletion: " + e.Message);
            }
        }
    }
}

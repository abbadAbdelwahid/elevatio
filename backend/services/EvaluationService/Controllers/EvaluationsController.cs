using EvaluationService.DTOs;
using EvaluationService.Models;
using EvaluationService.Services;
using Microsoft.AspNetCore.Mvc;

namespace EvaluationService.Controllers;

[ApiController]
[Route("api/evaluations")]
public class EvaluationController : ControllerBase
{
    private readonly IEvaluationService _evaluationService;

    public EvaluationController(IEvaluationService evaluationService)
    {
        _evaluationService = evaluationService;
    }

    [HttpPost("addEvaluations")]
    public async Task<IActionResult> AddEvaluations([FromBody] List<CreateEvaluationDto> evaluationsDto)
    {
        if (evaluationsDto == null || !evaluationsDto.Any())
        {
            return BadRequest("Invalid data.");
        }

        try
        {
            var evaluations = await _evaluationService.AddRangeAsync(evaluationsDto);
            return Ok(evaluations);
        }
        catch (Exception e)
        {
            return StatusCode(500, "An error occurred while adding evaluations: " + e.Message);
        }
    }

    [HttpGet("getEvaluationType/{evaluationId:int}")]
    public async Task<IActionResult> GetEvaluationType(int evaluationId)
    {
        try
        {
            var evaluationType = await _evaluationService.GetEvaluationType(evaluationId);
            if (evaluationType == null)
            {
                return NotFound("Evaluation type not found for the given evaluation ID.");
            }
            return Ok(evaluationType);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Error fetching evaluation type: " + e.Message);
        }
    }

    [HttpGet("getEvaluationsByFiliereId/{filiereId:int}")]
    public async Task<IActionResult> GetEvaluationsByFiliereId(int filiereId)
    {
        try
        {
            var evaluations = await _evaluationService.GetEvaluationsByFiliereIdAsync(filiereId);
            if (evaluations == null || !evaluations.Any())
            {
                return NotFound("No evaluations found for the given filière ID.");
            }
            return Ok(evaluations);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Error fetching evaluations: " + e.Message);
        }
    }

    [HttpGet("getEvaluationsByModuleId/{moduleId:int}")]
    public async Task<IActionResult> GetEvaluationsByModuleId(int moduleId)
    {
        try
        {
            var evaluations = await _evaluationService.GetEvaluationsByModuleIdAsync(moduleId);
            if (evaluations == null || !evaluations.Any())
            {
                return NotFound("No evaluations found for the given module ID.");
            }
            return Ok(evaluations);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Error fetching evaluations: " + e.Message);
        }
    }

    [HttpGet("getEvaluationsByRespondentId/{respondentId}")]
    public async Task<IActionResult> GetEvaluationsByRespondentId(string respondentId)
    {
        try
        {
            var evaluations = await _evaluationService.GetEvaluationsByRespondentIdAsync(respondentId);
            if (evaluations == null || !evaluations.Any())
            {
                return NotFound("No evaluations found for the given respondent ID.");
            }
            return Ok(evaluations);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Error fetching evaluations: " + e.Message);
        }
    }

    [HttpGet("getAllEvaluations")]
    public async Task<IActionResult> GetAllEvaluations()
    {
        try
        {
            var evaluations = await _evaluationService.GetAllEvaluationsAsync();
            return Ok(evaluations);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Error fetching evaluations: " + e.Message);
        }
    }

    [HttpGet("getEvaluationById/{evaluationId:int}")]
    public async Task<IActionResult> GetEvaluationById(int evaluationId)
    {
        try
        {
            var evaluation = await _evaluationService.GetEvaluationByIdAsync(evaluationId);
            if (evaluation == null)
            {
                return NotFound("Evaluation not found for the given evaluation ID.");
            }
            return Ok(evaluation);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Error fetching evaluation: " + e.Message);
        }
    }

    [HttpDelete("deleteEvaluationById/{evaluationId:int}")]
    public async Task<IActionResult> DeleteEvaluationById(int evaluationId)
    {
        try
        {
            var evaluation = await _evaluationService.DeleteEvaluationByIdAsync(evaluationId);
            if (evaluation == null)
            {
                return NotFound("Evaluation not found for the given evaluation ID.");
            }
            return Ok(evaluation);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Error deleting evaluation: " + e.Message);
        }
    }
    
    [HttpDelete("deleteEvaluationByRespondentId/{respondentId}")]
    public async Task<IActionResult> DeleteEvaluationByRespondentId(string respondentId)
    {
        try
        {
            var evaluations = await _evaluationService.DeleteEvaluationsByRespondentIdAsync(respondentId);
            if (evaluations == null || !evaluations.Any())
            {
                return NotFound("No evaluations found for the given respondent ID.");
            }

            return Ok(evaluations);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Error deleting evaluations: " + e.Message);
        }
    }    
    
    [HttpDelete("deleteEvaluationByFiliereId/{filiereId:int}")]
    public async Task<IActionResult> DeleteEvaluationByFiliereId(int filiereId)
    {
        try
        {
            var evaluations = await _evaluationService.DeleteEvaluationsByFiliereIdAsync(filiereId);
            if (evaluations == null || !evaluations.Any())
            {
                return NotFound("No evaluations found for the given filiere ID.");
            }

            return Ok(evaluations);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Error deleting evaluations: " + e.Message);
        }
    }   
    
    [HttpDelete("deleteEvaluationByModuleId/{moduleId:int}")]
    public async Task<IActionResult> DeleteEvaluationByModuleId(int moduleId)
    {
        try
        {
            var evaluations = await _evaluationService.DeleteEvaluationsByModuleIdAsync(moduleId);
            if (evaluations == null || !evaluations.Any())
            {
                return NotFound("No evaluations found for the given module ID.");
            }

            return Ok(evaluations);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Error deleting evaluations: " + e.Message);
        }
    }

    [HttpPut("updateEvaluation")]
    public async Task<IActionResult> UpdateEvaluation([FromBody] Evaluation evaluationUpdateDto)
    {
        if (evaluationUpdateDto == null)
        {
            return BadRequest("Invalid data.");
        }

        try
        {
            var updatedEvaluation = await _evaluationService.UpdateEvaluationAsync(evaluationUpdateDto);
            return Ok(updatedEvaluation);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Error updating evaluation: " + e.Message);
        }
    }

    [HttpPost("addEvaluation")]
    public async Task<IActionResult> AddEvaluation([FromBody] CreateEvaluationDto createEvaluationDto)
    {
        if (createEvaluationDto == null)
        {
            return BadRequest("Invalid data.");
        }

        try
        {
            var newEvaluation = await _evaluationService.AddEvaluationAsync(createEvaluationDto);
            return CreatedAtAction(nameof(GetEvaluationById), new { evaluationId = newEvaluation.EvaluationId }, newEvaluation);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Error adding evaluation: " + e.Message);
        }
    }
    
}


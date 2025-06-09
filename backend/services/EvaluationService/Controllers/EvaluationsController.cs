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
            return Ok(new { evaluationType = evaluationType});
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound("Error fetching evaluation: " + ex.Message);
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
            /*if (evaluations == null || !evaluations.Any())
            {
                return NotFound("No evaluations found for the given module ID.");
            }*/
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
            return Ok(evaluation);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound("Error fetching evaluation: " + ex.Message);
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
            return Ok(evaluation);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound("Error fetching evaluation: " + ex.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Error deleting evaluation: " + e.Message);
        }
    }
    
    [HttpDelete("deleteEvaluationsByRespondentId/{respondentId}")]
    public async Task<IActionResult> DeleteEvaluationsByRespondentId(string respondentId)
    {
        try
        {
            var evaluations = await _evaluationService.DeleteEvaluationsByRespondentIdAsync(respondentId);
            return Ok(evaluations);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Error deleting evaluations: " + e.Message);
        }
    }    
    
    [HttpDelete("deleteEvaluationsByFiliereId/{filiereId:int}")]
    public async Task<IActionResult> DeleteEvaluationsByFiliereId(int filiereId)
    {
        try
        {
            var evaluations = await _evaluationService.DeleteEvaluationsByFiliereIdAsync(filiereId);
            return Ok(evaluations);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Error deleting evaluations: " + e.Message);
        }
    }   
    
    [HttpDelete("deleteEvaluationsByModuleId/{moduleId:int}")]
    public async Task<IActionResult> DeleteEvaluationsByModuleId(int moduleId)
    {
        try
        {
            var evaluations = await _evaluationService.DeleteEvaluationsByModuleIdAsync(moduleId);
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
        catch (KeyNotFoundException ex)
        {
            return NotFound("Error updating evaluation: " + ex.Message);
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


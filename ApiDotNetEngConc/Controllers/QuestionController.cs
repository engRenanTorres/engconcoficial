
using DotnetAPI.DTOs;
using DotnetAPI.Models.Inharitance;
using DotnetAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;


namespace DotnetAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class QuestionController : ControllerBase
{
    private readonly ILogger<QuestionController> _logger;
    private readonly IQuestionService _questionService;

    public QuestionController(
      ILogger<QuestionController> logger,
      IQuestionService questionService
    )
    {
        _logger = logger;
        _questionService = questionService;
    }


    [AllowAnonymous]
    [HttpGet("{id}")]
    [ActionName("GetQuestion")]
    public async Task<ActionResult<BaseQuestion>> GetQuestion(int id)
    {
        _logger.LogInformation("GetQuestion has been called.");

        BaseQuestion? question = await _questionService.GetQuestion(id);

        return question != null ?
          Ok(question) :
          NotFound("Question id: " + id + " not found");
    }

    [AllowAnonymous]
    [HttpGet("")]
    public async Task<ActionResult<IEnumerable<BaseQuestion>>> GetQuestions()
    {
        _logger.LogInformation("GetQuestions has been called.");
        IEnumerable<BaseQuestion?> questions = await _questionService.GetAllQuestions();

        return Ok(questions);
    }

    [HttpPost("")]
    public async Task<ActionResult<BaseQuestion>> CreateQuestion(CreateQuestionDTO questionDTO)
    {
        _logger.LogInformation("CreateQuestion has been called.");

        if (questionDTO == null) return BadRequest("Question data is null.");
        string? userId = User?.FindFirst("userId")?.Value;
        if (userId == null) return NotFound("Please log a user");
        BaseQuestion? question =
        await _questionService.CreateQuestionOrThrow(questionDTO, userId);
        var actionName = nameof(GetQuestions);
        var routeValues = "";
        return CreatedAtAction(actionName,routeValues, question);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<BaseQuestion>> PatchQuestion(int id, [FromBody] UpdateQuestionDTO updateQuestionDTO)
    {
        _logger.LogInformation("PatchQuestions has been called.");
        try
        {
            var updatedQuestion = await _questionService.PatchQuestion(id, updateQuestionDTO);

            return Ok(updatedQuestion);
        }
        catch (Exception ex)
        {
            if (ex is WarningException)
            {
                _logger.LogError(ex, "Question not found while updating.");
                return NotFound("Question id: " + id + " not found");
            }
            _logger.LogError(ex, "An error occurred while deleting the question.");
        }
        throw new Exception("Error to delete Question");
    }


    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteQuestion(int id)
    {
        _logger.LogInformation("Delete Question Controller has been called.");

        try
        {
            var deleteQuestion = await _questionService.DeleteQuestion(id);
            if (deleteQuestion)
            {
                return NoContent();
            };
        }
        catch (Exception ex)
        {
            if (ex is WarningException)
            {
                _logger.LogError(ex, "Question not found while deleting.");
                return NotFound("Question id: " + id + " not found");
            }
            _logger.LogError(ex, "An error occurred while deleting the question.");
        }
        throw new Exception("Error to delete Question");
    }
}

